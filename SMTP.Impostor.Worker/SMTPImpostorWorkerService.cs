using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;
using SMTP.Impostor.Worker.Actions;
using SMTP.Impostor.Worker.Actions.State;
using SMTP.Impostor.Worker.Hubs;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorWorkerService : BackgroundService
    {
        readonly ILogger<SMTPImpostorWorkerService> _logger;
        readonly SMTPImpostor _impostor;
        readonly SMTPImpostorHubService _hub;
        readonly IActionExecutor _executor;
        readonly ISMTPImpostorHostSettingsStore _hostsSettings;
        readonly ToastNotifier _notifier;
        readonly ToastNotification _startNotification;

        IDisposable _impostorEvents;

        public SMTPImpostorWorkerService(
            ILogger<SMTPImpostorWorkerService> logger,
            SMTPImpostor impostor,
            SMTPImpostorHubService hub,
            IActionExecutor executor,
            ISMTPImpostorHostSettingsStore hostsSettings)
        {
            _logger = logger;
            _impostor = impostor;
            _hub = hub;
            _executor = executor;
            _hostsSettings = hostsSettings;

            if (Environment.UserInteractive)
            {
                try
                {
                    _notifier = ToastNotificationManager.CreateToastNotifier();
                    _startNotification = Notify(
                        "Worker is running",
                        START_NOTIFICATION_ID, false);
                }
                catch (Exception) { } // ignore fails when running as console
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _impostorEvents = _impostor.Events.Subscribe(async e =>
            {
                _logger.LogInformation($"{e.HostId} {e.GetType().Name}");

                if (e is SMTPImpostorMessageReceivedEvent mre)
                {
                    var host = _impostor.Hosts[e.HostId];
                    await host.Messages.PutAsync(mre.Data);

                    await _hub.SendAsync(
                        new HostMessageReceived(host.Settings.Id, mre.Data.Map())
                        );
                }
                else if (e is SMTPImpostorMessageRemovedEvent mde)
                {
                    await _hub.SendAsync(
                        new HostMessageRemoved(mde.HostId, mde.MessageId)
                        );
                }
                else if (e is SMTPImpostorMessageAddedEvent mae)
                {
                    await _hub.SendAsync(
                        new HostMessageRemoved(mae.HostId, mae.MessageId)
                        );
                }
                else if (e is SMTPImpostorHostStateChangeEvent
                    || e is SMTPImpostorHostUpdatedEvent)
                {
                    var status = await _executor
                        .ExecuteAsync<LoadWorkerStateAction, WorkerState>();
                    var hostState = status.Hosts
                        .First(h => h.Id == e.HostId);
                    await _hub.SendAsync(hostState);

                    await _hostsSettings.SaveAsync(status.ToSettings());
                }
                else if (e is SMTPImpostorHostRemovedEvent
                    || e is SMTPImpostorHostAddedEvent)
                {
                    var status = await _executor
                        .ExecuteAsync<LoadWorkerStateAction, WorkerState>();
                    await _hub.SendAsync(status);

                    await _hostsSettings.SaveAsync(status.ToSettings());
                }
            });

            var settings = await _hostsSettings.LoadAsync();

            foreach (var hostSetttings in settings)
            {
                _impostor.AddHost(hostSetttings);
            }
        }

        const string START_NOTIFICATION_ID = "SMTP_START_ID";

        ToastNotification Notify(
            string text, string id,
            bool suppressPopup = false)
        {
            if (_notifier == null) return null;

            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            var binding = template.GetElementsByTagName("binding").Item(0);
            
            var actions = template.CreateElement("actions");
            binding.AppendChild(actions);

            var action = template.CreateElement("action");
            actions.AppendChild(action);
            action.SetAttribute("content", "Open UI");
            action.SetAttribute("arguments", "open");
            action.SetAttribute("activationType", "background");

            action = template.CreateElement("action");
            actions.AppendChild(action);
            action.SetAttribute("content", "Stop Worker");
            action.SetAttribute("arguments", "stop");
            action.SetAttribute("activationType", "background");

            var textNodes = template.GetElementsByTagName("text");
            textNodes.Item(0).InnerText = text;

            var templateString = template.GetXml();

            var notification = new ToastNotification(template);
            notification.Tag = id;
            notification.SuppressPopup = suppressPopup;

            _notifier.Show(notification);

            return notification;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _impostorEvents.Dispose();
            if (_startNotification != null)
                _notifier.Hide(_startNotification);

            return Task.CompletedTask;
        }
    }
}
