using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor
{
    public class SMTPImpostor : IDisposable
    {
        readonly ILoggerFactory _loggerFactory;
        readonly ISMTPImpostorSettings _settings;
        readonly ISMTPImpostorSocketHandlerProvider _handlerProvider;
        readonly Subject<ISMTPImpostorEvent> _events;

        public SMTPImpostor(
            ILoggerFactory loggerFactory,
            ISMTPImpostorSettings settings,
            ISMTPImpostorSocketHandlerProvider handlerProvider)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _handlerProvider = handlerProvider ?? throw new ArgumentNullException(nameof(handlerProvider));
            _events = new Subject<ISMTPImpostorEvent>();

            foreach (var hostSettings in _settings.Hosts)
                AddHost(hostSettings);
        }

        public ISMTPImpostorHost CreateHost()
        {
            return new SMTPImpostorHost(
                _handlerProvider,
                _loggerFactory.CreateLogger<SMTPImpostorHost>()
                );
        }

        public IImmutableDictionary<SMTPImpostorHostSettings, ISMTPImpostorHost> Hosts { get; private set; }
            = ImmutableDictionary<SMTPImpostorHostSettings, ISMTPImpostorHost>.Empty;
        public IObservable<ISMTPImpostorEvent> Events => _events;

        public ISMTPImpostorHost AddHost(SMTPImpostorHostSettings hostSettings)
        {
            var host = CreateHost();
            Hosts = Hosts.Add(hostSettings, host);

            host.Configure(hostSettings);
            host.Events.Subscribe(_events);

            return host;
        }

        public void RemoveHost(SMTPImpostorHostSettings hostSettings)
        {
            Hosts = Hosts.Remove(hostSettings);
        }

        void IDisposable.Dispose()
        {
            if (Hosts != null)
            {
                foreach (var host in Hosts.ToArray())
                {
                    host.Value.Dispose();
                    RemoveHost(host.Key);
                }
                Hosts = null;
            }
        }
    }
}
