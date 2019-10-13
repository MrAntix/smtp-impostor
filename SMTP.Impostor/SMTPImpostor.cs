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
        readonly Subject<ISMTPImpostorEvent> _events;

        public SMTPImpostor(
            ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _events = new Subject<ISMTPImpostorEvent>();
        }

        public ISMTPImpostorHost CreateHost()
        {
            return new SMTPImpostorHost(
                _loggerFactory.CreateLogger<SMTPImpostorHost>()
                );
        }

        public IImmutableDictionary<SMTPImpostorHostSettings, ISMTPImpostorHost> Hosts { get; private set; }
            = ImmutableDictionary<SMTPImpostorHostSettings, ISMTPImpostorHost>.Empty;
        public IObservable<ISMTPImpostorEvent> Events => _events;

        public ISMTPImpostorHost AddHost(SMTPImpostorHostSettings hostSettings)
        {
            var host = CreateHost();
            host.Configure(hostSettings);

            Hosts = Hosts.Add(host.Settings, host);
            _events.OnNext(new SMTPImpostorHostAddedEvent(host.Id));

            host.Events.Subscribe(_events);

            if (hostSettings.Start) host.Start();

            return host;
        }

        public void RemoveHost(SMTPImpostorHostSettings hostSettings)
        {
            if (Hosts.TryGetValue(hostSettings, out var host))
            {
                host.Stop();
                Hosts = Hosts.Remove(hostSettings);
                _events.OnNext(new SMTPImpostorHostRemovedEvent(host.Id));
            }
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
