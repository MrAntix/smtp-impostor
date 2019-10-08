using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Store;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorService : BackgroundService
    {
        readonly ILogger<SMTPImpostorService> _logger;
        readonly SMTPImpostor _impostor;
        readonly ISMTPImpostorStore _store;
        IDisposable _events;

        public SMTPImpostorService(
            ILogger<SMTPImpostorService> logger,
            SMTPImpostor impostor,
            ISMTPImpostorStore store)
        {
            _logger = logger;
            _impostor = impostor;
            _store = store;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _events = _impostor.Events.Subscribe(async e =>
            {
                _logger.LogInformation($"{e.HostSettings} {e.GetType().Name}");

                if (e is SMTPImpostorMessageReceivedEvent mre)
                {
                    await _store.PutAsync(e.HostSettings.FriendlyName, mre.Data);
                    GC.Collect();
                }
            });

            var host = _impostor.AddHost(
                new SMTPImpostorHostSettings(
                    hostName: "127.0.0.1",
                    port: 25));
            host.Start();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _events.Dispose();

            return Task.CompletedTask;
        }
    }
}
