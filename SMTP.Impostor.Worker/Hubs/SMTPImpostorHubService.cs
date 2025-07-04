using Microsoft.Extensions.Logging;
using SMTP.Impostor.Worker.Actions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs
{
    public class SMTPImpostorHubService :
        IDisposable
    {
        readonly ILogger<SMTPImpostorHubService> _logger;
        readonly IActionExecutor _executor;
        readonly SMTPImpostorSerialization _serialization;
        readonly SemaphoreSlim _messageSemaphore;
        readonly BehaviorSubject<IImmutableList<ISMTPImpostorHubClient>> _clients;
        readonly Subject<SMTPImpostorHubMessage> _messages;

        public SMTPImpostorHubService(
            ILogger<SMTPImpostorHubService> logger,
            IActionExecutor executor,
            SMTPImpostorSerialization serialization)
        {
            _logger = logger;
            _executor = executor;
            _serialization = serialization;
            _clients = new BehaviorSubject<IImmutableList<ISMTPImpostorHubClient>>(ImmutableList<ISMTPImpostorHubClient>.Empty);
            _messages = new Subject<SMTPImpostorHubMessage>();
            _messageSemaphore = new SemaphoreSlim(1);
        }

        public IObservable<IImmutableList<ISMTPImpostorHubClient>> Clients => _clients;
        public IObservable<SMTPImpostorHubMessage> Messages => _messages;

        public async Task ConnectAsync(ISMTPImpostorHubClient client)
        {
            _clients.OnNext(_clients.Value.Add(client));
            await ListenAsync(client);
            _clients.OnNext(_clients.Value.Remove(client));
        }

        async Task ListenAsync(ISMTPImpostorHubClient client)
        {
            try
            {
                while (client.State == WebSocketState.Open)
                {
                    try
                    {
                        var data = await client.ReceiveAsync();

                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            var message = Newtonsoft.Json.JsonConvert
                                .DeserializeObject<SMTPImpostorHubMessage>(data);
                            _messages.OnNext(message);

                            try
                            {
                                var result = await _executor.ExecuteAsync(message.Type, message.Data);
                                if (result != null
                                    && result != ActionNull.Instance)
                                {
                                    await client.SendAsync(
                                        _serialization.Serialize(
                                            CreateMessageFrom(result)
                                        ));
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Executor failed\n{data}");
                            }
                        }
                    }
                    catch (WebSocketException ex)
                        when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        _logger.LogDebug(ex, "ListenAsync");
                    }
                }
            }
            catch (Exception ex)
            {
                if (client.State == WebSocketState.Open)
                {
                    _logger.LogError(ex, $"Could not receive message");
                    await client.CloseAsync();
                }
            }
        }

        public async Task SendAsync(
            object model,
            IEnumerable<ISMTPImpostorHubClient> clients = null)
        {
            await Send(
                 model.GetType().Name,
                 model,
                 clients
                );
        }

        public async Task Send(
            string type,
            object model,
            IEnumerable<ISMTPImpostorHubClient> clients = null)
        {
            await SendMessageAsync(
                new SMTPImpostorHubMessage(
                    type,
                    _serialization.Serialize(model)
                    ),
                clients);
        }

        public async Task SendMessageAsync(
            SMTPImpostorHubMessage message,
            IEnumerable<ISMTPImpostorHubClient> clients = null)
        {
            await EnqueueMessage(async () =>
            {
                var data = _serialization.Serialize(message);

                foreach (var client in clients ?? _clients.Value)
                {
                    try
                    {
                        await client.SendAsync(data);
                    }
                    catch (Exception ex)
                    {
                        if (client.State == WebSocketState.Open)
                        {
                            _logger.LogError(ex, data);
                            await client.CloseAsync();
                        }
                    }
                }
            });
        }

        SMTPImpostorHubMessage CreateMessageFrom(object model)
        {
            return new SMTPImpostorHubMessage(
                model.GetType().Name,
                _serialization.Serialize(model)
                );
        }

        async Task EnqueueMessage(Func<Task> taskGenerator)
        {
            await _messageSemaphore.WaitAsync();
            try
            {
                await taskGenerator();
            }
            finally
            {
                _messageSemaphore.Release();
            }
        }

        public void Dispose()
        {
            foreach (var client in _clients.Value)
                try
                {
                    client.CloseAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hub Dispose");
                }

            _clients.OnNext(ImmutableList<ISMTPImpostorHubClient>.Empty);
            _clients.OnCompleted();
        }
    }
}
