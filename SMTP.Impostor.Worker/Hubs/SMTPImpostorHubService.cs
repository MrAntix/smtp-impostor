using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SMTP.Impostor.Worker.Hubs
{
    public class SMTPImpostorHubService :
        IDisposable
    {
        readonly ILogger<SMTPImpostorHubService> _logger;
        readonly SemaphoreSlim _messageSemaphore;
        readonly BehaviorSubject<IImmutableList<ISMTPImpostorHubClient>> _clients;
        readonly Subject<SMTPImpostorHubMessage> _messages;

        public SMTPImpostorHubService(
            ILogger<SMTPImpostorHubService> logger)
        {
            _logger = logger;
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
                    var data = await client.ReceiveAsync();

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        var message = Newtonsoft.Json.JsonConvert
                            .DeserializeObject<SMTPImpostorHubMessage>(data);
                        _messages.OnNext(message);
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

        public async Task SendMessage(
            SMTPImpostorHubMessage message,
            IEnumerable<ISMTPImpostorHubClient> clients = null)
        {
            await EnqueueMessage(async () =>
            {
                var data = JsonSerializer.Serialize(message);

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
