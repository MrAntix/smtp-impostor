using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMTP.Impostor.Events;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;
using SMTP.Impostor.Worker.Actions;
using SMTP.Impostor.Worker.Actions.State;
using SMTP.Impostor.Worker.Hubs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace SMTP.Impostor.Worker
{
    public class SMTPImpostorWorkerService : BackgroundService
    {
        const string MUTEX_NAME = "SMTP.Impostor.Worker";

        readonly ILogger<SMTPImpostorWorkerService> _logger;
        readonly ISMTPImpostorWorkerSettings _settings;
        readonly SMTPImpostor _impostor;

        readonly SMTPImpostorHubService _hub;
        readonly IActionExecutor _executor;
        readonly ISMTPImpostorHostSettingsStore _hostsSettings;
        ToastNotifier _notifier;

        IDisposable _impostorEvents;
        static Mutex _mutex;

        public SMTPImpostorWorkerService(
            ILogger<SMTPImpostorWorkerService> logger,
            ISMTPImpostorWorkerSettings settings,
            SMTPImpostor impostor,
            SMTPImpostorHubService hub,
            IActionExecutor executor,
            ISMTPImpostorHostSettingsStore hostsSettings)
        {
            _logger = logger;
            _settings = settings;
            _impostor = impostor;
            _hub = hub;
            _executor = executor;
            _hostsSettings = hostsSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken _)
        {
            if (Mutex.TryOpenExisting(MUTEX_NAME, out var __))
            {
                _logger.LogWarning("Service is already running, shutting down");
                await StopAsync(CancellationToken.None);
                return;
            }

            if (Environment.UserInteractive)
            {
                if (UWPHelper.IsUwp())
                {
                    try
                    {
                        _notifier = ToastNotificationManager.CreateToastNotifier();
                        UWPHelper.SendStartupMessage(_notifier, _settings);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending UWP start notification");
                    }
                }
            }

            _mutex = new Mutex(false, MUTEX_NAME);
            _impostorEvents = _impostor.Events.Subscribe(async e =>
            {
                _logger.LogInformation($"{e.GetType().Name}");
                try
                {
                    if (e is SMTPImpostorStoppedEvent)
                    {
                        await _hub.SendAsync(new WorkerState(null, null));
                        await StopAsync(CancellationToken.None);
                    }
                    else if (e is SMTPImpostorMessageReceivedEvent mre)
                    {
                        var host = _impostor.Hosts[mre.HostId];
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
                            new HostMessageAdded(mae.HostId, mae.MessageId)
                            );
                    }
                    else if (
                        (e is SMTPImpostorHostStateChangeEvent || e is SMTPImpostorHostUpdatedEvent)
                        && e is ISMTPImpostorHostEvent he)
                    {
                        var status = await _executor
                            .ExecuteAsync<LoadWorkerStateAction, WorkerState>();
                        var hostState = status.Hosts
                            .FirstOrDefault(h => h.Id == he.HostId);
                        if (hostState == null) return;

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
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Event Error");
                }
            });

            var settings = await _hostsSettings.LoadAsync();
            if (settings != null)
                foreach (var hostSetttings in settings)
                {
                    _impostor.AddHost(hostSetttings);
                }
        }

        public override Task StopAsync(CancellationToken _)
        {
            if (_mutex != null)
            {
                _mutex.Close();
                _mutex = null;
            }
            if (_impostorEvents != null)
            {
                _impostorEvents.Dispose();
                _impostorEvents = null;
            }

            Environment.Exit(0);

            return Task.CompletedTask;
        }
    }
}
