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
        public SMTPImpostorHostStateChangeEvent StoppedEvent;
        public SMTPImpostorHostStateChangeEvent StartedEvent;
        public SMTPImpostorHostStateChangeEvent ReceivingEvent;
        public SMTPImpostorHostStateChangeEvent ErroredEvent;

        readonly ILogger<SMTPImpostorHost> _logger;
        bool _started;

        Subject<ISMTPImpostorEvent> _events;
        IObservable<ISMTPImpostorEvent> ISMTPImpostorHost.Events => _events;

        public SMTPImpostorHost(
            ILogger<SMTPImpostorHost> logger = null)
        {
            Id = Guid.NewGuid();
            _logger = logger ?? NullLogger<SMTPImpostorHost>.Instance;
            _events = new Subject<ISMTPImpostorEvent>();
        }

        public Guid Id { get; }

        public SMTPImpostorHostSettings Settings { get; private set; }
        public SMTPImpostorHostStates State { get; private set; }
        void RaiseStateChange(SMTPImpostorHostStateChangeEvent e)
        {
            _events.OnNext(e);
            State = e.Data;
        }

        void ISMTPImpostorHost.Configure(SMTPImpostorHostSettings settings)
        {
            Settings = settings ??
                throw new ArgumentNullException(nameof(settings));

            StoppedEvent = new SMTPImpostorHostStateChangeEvent(settings, SMTPImpostorHostStates.Stopped);
            StartedEvent = new SMTPImpostorHostStateChangeEvent(settings, SMTPImpostorHostStates.Started);
            ReceivingEvent = new SMTPImpostorHostStateChangeEvent(settings, SMTPImpostorHostStates.Receiving);
            ErroredEvent = new SMTPImpostorHostStateChangeEvent(settings, SMTPImpostorHostStates.Errored);

            RaiseStateChange(StoppedEvent);
        }

        void ISMTPImpostorHost.Start()
        {
            if (_started)
                throw new SMTPImpostorHostAlreadyStarted(Settings);

            _started = true;
            RaiseStateChange(StartedEvent);

            Task.Run(() =>
            {
                try
                {
                    var listener = new TcpListener(
                        IPAddress.Parse(Settings.IP),
                        Settings.Port);
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
                                    new SMTPImpostorMessageReceivedEvent(Settings, message)
                                    );

                                RaiseStateChange(StartedEvent);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"host {Settings.Name}");
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
                    _logger.LogError(ex, $"host {Settings.Name}");
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
