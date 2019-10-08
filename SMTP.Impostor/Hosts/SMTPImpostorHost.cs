using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHost : ISMTPImpostorHost
    {
        ISMTPImpostorHost This => this;

        readonly ILogger<SMTPImpostorHost> _logger;

        Subject<ISMTPImpostorEvent> _events;
        IObservable<ISMTPImpostorEvent> ISMTPImpostorHost.Events => _events;

        SMTPImpostorHostSettings _hostSettings;
        bool _started;
        public SMTPImpostorHostStateChangeEvent StoppedEvent;
        public SMTPImpostorHostStateChangeEvent StartedEvent;
        public SMTPImpostorHostStateChangeEvent ReceivingEvent;
        public SMTPImpostorHostStateChangeEvent ErroredEvent;


        public SMTPImpostorHost(
            ILogger<SMTPImpostorHost> logger = null)
        {
            _logger = logger ?? NullLogger<SMTPImpostorHost>.Instance;
            _events = new Subject<ISMTPImpostorEvent>();
        }

        public SMTPImpostorHostStates State { get; private set; }
        void RaiseStateChange(SMTPImpostorHostStateChangeEvent e)
        {
            _events.OnNext(e);
            State = e.Data;
        }

        void ISMTPImpostorHost.Configure(SMTPImpostorHostSettings hostSettings)
        {
            _hostSettings = hostSettings ??
                throw new ArgumentNullException(nameof(hostSettings));

            StoppedEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Stopped);
            StartedEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Started);
            ReceivingEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Receiving);
            ErroredEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Errored);

            RaiseStateChange(StoppedEvent);
        }

        void ISMTPImpostorHost.Start()
        {
            if (_started)
                throw new SMTPImpostorHostAlreadyStarted(_hostSettings);

            _started = true;
            RaiseStateChange(StartedEvent);

            Task.Run(() =>
            {
                try
                {
                    var listener = new TcpListener(
                        IPAddress.Parse(_hostSettings.HostName),
                        _hostSettings.Port);
                    listener.Start();

                    while (_started)
                    {
                        var client = listener.AcceptTcpClient();
                        RaiseStateChange(ReceivingEvent);

                        Task.Run(async () =>
                        {
                            try
                            {
                                RaiseStateChange(ReceivingEvent);

                                using var handler = new SocketHandler(SocketWrapper.Wrap(client.Client));

                                var message = await handler.HandleAsync();
                                _events.OnNext(
                                    new SMTPImpostorMessageReceivedEvent(_hostSettings, message)
                                    );

                                RaiseStateChange(StartedEvent);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"host {_hostSettings.FriendlyName}");
                                RaiseStateChange(ErroredEvent);
                            }
                            finally
                            {
                                client.Dispose();
                            }
                        });
                    }

                    listener.Stop();
                }
                catch (SocketException ex)
                {
                    _logger.LogError(ex, $"host {_hostSettings.FriendlyName}");
                    This.Stop();

                    RaiseStateChange(ErroredEvent);
                }
            });
        }

        void ISMTPImpostorHost.Stop()
        {
            if (_events != null)
            {
                RaiseStateChange(StoppedEvent);
                _events.OnCompleted();
                _events = null;
            }

            _started = false;
        }

        void IDisposable.Dispose()
        {
            This.Stop();
        }
    }
}
