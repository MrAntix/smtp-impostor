using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Store;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorService : BackgroundService
    {
        readonly SMTPImpostor _impostor;
        readonly ISMTPImpostorStore _store;

        public SMTPImpostorService(
            SMTPImpostor impostor,
            ISMTPImpostorStore store)
        {
            _impostor = impostor;
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _impostor.Events.Subscribe(e =>
            {
                Console.WriteLine($"{e.HostSettings} {e.GetType().Name}");
                Console.WriteLine(JsonSerializer.Serialize(e, e.GetType()));

                if (e is SMTPImpostorMessageReceivedEvent mre)
                {
                    _store.PutAsync(e.HostSettings.FriendlyName, mre.Data);
                }
            });

            var host = _impostor.AddHost(
                new SMTPImpostorHostSettings(
                    hostName: "127.0.0.1",
                    port: 25));
            host.Start();
        }
    }
}
