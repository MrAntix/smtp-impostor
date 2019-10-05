using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Threading;
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
        readonly ISMTPImpostorSocketHandlerProvider _handlerProvider;

        BehaviorSubject<ISMTPImpostorEvent> _events;
        IObservable<ISMTPImpostorEvent> ISMTPImpostorHost.Events => _events;

        SMTPImpostorHostSettings _hostSettings;
        bool _started;
        public SMTPImpostorHostStateChangeEvent StoppedEvent;
        public SMTPImpostorHostStateChangeEvent StartedEvent;
        public SMTPImpostorHostStateChangeEvent ReceivingEvent;
        public SMTPImpostorHostStateChangeEvent ErroredEvent;


        public SMTPImpostorHost(
            ISMTPImpostorSocketHandlerProvider handlerProvider,
            ILogger<SMTPImpostorHost> logger = null)
        {
            _handlerProvider = handlerProvider;
            _logger = logger ?? NullLogger<SMTPImpostorHost>.Instance;
            _events = new BehaviorSubject<ISMTPImpostorEvent>(null);
        }

        void ISMTPImpostorHost.Configure(SMTPImpostorHostSettings hostSettings)
        {
            _hostSettings = hostSettings ??
                throw new ArgumentNullException(nameof(hostSettings));

            StoppedEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Stopped);
            StartedEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Started);
            ReceivingEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Receiving);
            ErroredEvent = new SMTPImpostorHostStateChangeEvent(hostSettings, SMTPImpostorHostStates.Errored);

            _events.OnNext(StoppedEvent);
        }

        void ISMTPImpostorHost.Start()
        {
            if (_started)
                throw new SMTPImpostorHostAlreadyStarted(_hostSettings);

            _started = true;
            _events.OnNext(StartedEvent);

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
                        _events.OnNext(ReceivingEvent);

                        var thread = new Thread(async () =>
                            {
                                try
                                {
                                    using var socket = SocketWrapper.Wrap(client.Client);
                                    await _handlerProvider.HandleAsync(
                                        socket,
                                        m => _events.OnNext(new SMTPImpostorMessageReceivedEvent(_hostSettings, m)
                                        ));

                                    _events.OnNext(StartedEvent);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"host {_hostSettings.FriendlyName}");

                                    Console.WriteLine($"Exception: {ex}");
                                }
                            });

                        thread.Start();
                    }

                    listener.Stop();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"SocketException: {ex}");
                    This.Stop();

                    _events.OnNext(ErroredEvent);
                }
            });
        }

        void ISMTPImpostorHost.Stop()
        {
            if (_events != null)
            {
                _events.OnNext(StoppedEvent);
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
