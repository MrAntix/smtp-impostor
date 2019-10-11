using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Store;
using SMTP.Impostor.Worker.Hubs;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorService : BackgroundService
    {
        readonly ILogger<SMTPImpostorService> _logger;
        readonly SMTPImpostor _impostor;
        readonly SMTPImpostorHubService _hub;
        readonly ISMTPImpostorStore _store;

        IDisposable _impostorEvents;
        IDisposable _hubMessages;

        public SMTPImpostorService(
            ILogger<SMTPImpostorService> logger,
            SMTPImpostor impostor,
            SMTPImpostorHubService hub,
            ISMTPImpostorStore store)
        {
            _logger = logger;
            _impostor = impostor;
            _hub = hub;
            _store = store;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _impostorEvents = _impostor.Events.Subscribe(async e =>
            {
                _logger.LogInformation($"{e.HostSettings} {e.GetType().Name}");

                if (e is SMTPImpostorMessageReceivedEvent mre)
                {
                    await _store.PutAsync(e.HostSettings.Name, mre.Data);
                    await _hub.SendMessage(
                        new SMTPImpostorHubMessage(
                            "MessageRecieved",
                            JsonSerializer.Serialize(new
                            {
                                mre.Data.Date,
                                From = mre.Data.From.Address,
                                mre.Data.Subject
                            })
                        ));
                    GC.Collect();
                }
            });

            _hubMessages = _hub.Messages.Subscribe(async e =>
            {

            });

            var host = _impostor.AddHost(
                new SMTPImpostorHostSettings(
                    ip: "127.0.0.1",
                    port: 25));
            host.Start();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _impostorEvents.Dispose();
            _hubMessages.Dispose();

            return Task.CompletedTask;
        }
    }
}
