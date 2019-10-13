﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;
using SMTP.Impostor.Worker.Actions;
using SMTP.Impostor.Worker.Actions.State;
using SMTP.Impostor.Worker.Hubs;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorWorkerService : BackgroundService
    {
        readonly ILogger<SMTPImpostorWorkerService> _logger;
        readonly SMTPImpostorSerialization _serialization;
        readonly SMTPImpostor _impostor;
        readonly SMTPImpostorHubService _hub;
        readonly IActionExecutor _executor;
        readonly ISMTPImpostorMessagesStore _store;
        readonly ISMTPImpostorHostSettingsStore _hostsSettings;

        IDisposable _impostorEvents;

        public SMTPImpostorWorkerService(
            ILogger<SMTPImpostorWorkerService> logger,
            SMTPImpostorSerialization serialization,
            SMTPImpostor impostor,
            SMTPImpostorHubService hub,
            IActionExecutor executor,
            ISMTPImpostorMessagesStore store,
            ISMTPImpostorHostSettingsStore hostsSettings)
        {
            _logger = logger;
            _serialization = serialization;
            _impostor = impostor;
            _hub = hub;
            _executor = executor;
            _store = store;
            _hostsSettings = hostsSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
                            _serialization.Serialize(new
                            {
                                mre.Data.Date,
                                From = mre.Data.From.Address,
                                mre.Data.Subject
                            })
                        ));
                    GC.Collect();
                }
                else if (e is SMTPImpostorHostStateChangeEvent hsce)
                {
                    var status = await _executor.ExecuteAsync(GetStatusAction.Name);
                    await _hub.SendMessage(
                        new SMTPImpostorHubMessage(
                            "Status",
                            _serialization.Serialize(status)
                        ));
                }
            });

            var settings = await _hostsSettings.LoadAsync();
            await _hostsSettings.SaveAsync(settings);

            foreach (var hostSetttings in settings)
            {
                var host = _impostor.AddHost(hostSetttings);
                host.Start();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _impostorEvents.Dispose();

            return Task.CompletedTask;
        }
    }
}
