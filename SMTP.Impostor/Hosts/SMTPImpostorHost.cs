using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading.Tasks;

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
        TcpListener _listener;
        bool _listenerStarted = false;

        readonly Subject<ISMTPImpostorEvent> _events;
        readonly IDisposable _messagesSubscription;
        IObservable<ISMTPImpostorEvent> ISMTPImpostorHost.Events => _events;

        public SMTPImpostorHost(
            ILogger<SMTPImpostorHost> logger,
            SMTPImpostorHostSettings settings,
            ISMTPImpostorMessagesStore messages
            )
        {
            _logger = logger ?? NullLogger<SMTPImpostorHost>.Instance;
            _events = new Subject<ISMTPImpostorEvent>();

            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Messages = messages;
            _messagesSubscription = Messages.Events.Subscribe(_events.OnNext);

            StoppedEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStatus.Stopped);
            StartedEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStatus.Started);
            ReceivingEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStatus.Receiving);
            ErroredEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStatus.Errored);

            RaiseStateChange(StoppedEvent);
        }

        public SMTPImpostorHostSettings Settings { get; }
        public ISMTPImpostorMessagesStore Messages { get; }
        public SMTPImpostorHostStatus State { get; private set; }
        void RaiseStateChange(SMTPImpostorHostStateChangeEvent e)
        {
            State = e.Data;
            _events?.OnNext(e);
        }

        void ISMTPImpostorHost.Start()
        {
            if (_listenerStarted)
                throw new SMTPImpostorHostAlreadyStarted(Settings);

            RaiseStateChange(StartedEvent);

            Task.Run(async () =>
            {
                try
                {
                    _listener = new TcpListener(
                        IPAddress.Parse(Settings.IP),
                        Settings.Port);
                    _listener.Start();
                    _listenerStarted = true;

                    RaiseStateChange(StartedEvent);

                    do
                    {

                        using var client = await _listener.AcceptTcpClientAsync();
                        RaiseStateChange(ReceivingEvent);

                        var handler = new SocketHandler(
                           SocketWrapper.Wrap(client.Client),
                           _logger
                           );

                        while (client.Connected)
                        {
                            try
                            {
                                await handler.HandleAsync(
                                    message => _events.OnNext(
                                        new SMTPImpostorMessageReceivedEvent(Settings, message)
                                    )
                                );
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"host {Settings.Name}");
                                RaiseStateChange(ErroredEvent);
                            }
                        };

                    } while (_listenerStarted);

                    RaiseStateChange(StoppedEvent);
                }
                catch (ObjectDisposedException) { }
                catch (SocketException ex)
                {
                    switch (ex.SocketErrorCode)
                    {
                        case SocketError.OperationAborted:
                        case SocketError.Interrupted:
                            break;
                        default:

                            _logger.LogError(ex, $"host {Settings.Name}");

                            This.Stop();
                            RaiseStateChange(ErroredEvent);
                            break;
                    }
                }
            });
        }

        void ISMTPImpostorHost.Stop()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
            }
            _listenerStarted = false;

            RaiseStateChange(StoppedEvent);
        }

        bool _disposed = false;

        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _events.OnCompleted();
                _messagesSubscription.Dispose();
                Messages.Dispose();
            }

            This.Stop();
        }
    }
}
