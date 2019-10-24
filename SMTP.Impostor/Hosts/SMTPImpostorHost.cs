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
        TcpListener _listener;
        bool _listenerStarted = false;

        Subject<ISMTPImpostorEvent> _events;
        IObservable<ISMTPImpostorEvent> ISMTPImpostorHost.Events => _events;

        public SMTPImpostorHost(
            ILogger<SMTPImpostorHost> logger,
            SMTPImpostorHostSettings settings)
        {
            _logger = logger ?? NullLogger<SMTPImpostorHost>.Instance;
            _events = new Subject<ISMTPImpostorEvent>();

            Settings = settings ??
                throw new ArgumentNullException(nameof(settings));

            StoppedEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStates.Stopped);
            StartedEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStates.Started);
            ReceivingEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStates.Receiving);
            ErroredEvent = new SMTPImpostorHostStateChangeEvent(settings.Id, SMTPImpostorHostStates.Errored);

            RaiseStateChange(StoppedEvent);
        }

        public SMTPImpostorHostSettings Settings { get; private set; }
        public SMTPImpostorHostStates State { get; private set; }
        void RaiseStateChange(SMTPImpostorHostStateChangeEvent e)
        {
            State = e.Data;
            if (_events != null) _events.OnNext(e);
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

                    do
                    {
                        var client = await _listener.AcceptTcpClientAsync();
                        RaiseStateChange(ReceivingEvent);

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
                    } while (_listenerStarted);
                }
                catch (ObjectDisposedException) { }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.Interrupted)
                    {
                        _logger.LogError(ex, $"host {Settings.Name}");

                        This.Stop();
                        RaiseStateChange(ErroredEvent);
                    }
                }
            });
        }

        void ISMTPImpostorHost.Stop()
        {
            if (_listener != null)
                _listener.Stop();
            _listenerStarted = false;

            RaiseStateChange(StoppedEvent);
        }

        void IDisposable.Dispose()
        {
            if (_events != null)
            {
                _events.OnCompleted();
                _events = null;
            }

            This.Stop();
        }
    }
}
