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
    public class SMTPImpostor : ISMTPImpostorHostProvider, IDisposable
    {
        readonly ILoggerFactory _loggerFactory;
        readonly Subject<ISMTPImpostorEvent> _events;

        public SMTPImpostor(
            ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _events = new Subject<ISMTPImpostorEvent>();
        }

        public ISMTPImpostorHost CreateHost(
            SMTPImpostorHostSettings settings)
        {
            return new SMTPImpostorHost(
                _loggerFactory.CreateLogger<SMTPImpostorHost>(),
                settings
                );
        }

        public IImmutableDictionary<Guid, ISMTPImpostorHost> Hosts { get; private set; }
            = ImmutableDictionary<Guid, ISMTPImpostorHost>.Empty;
        public IObservable<ISMTPImpostorEvent> Events => _events;

        public ISMTPImpostorHost AddHost(SMTPImpostorHostSettings hostSettings)
        {
            var host = CreateHost(hostSettings);

            Hosts = Hosts.Add(host.Settings.Id, host);
            _events.OnNext(new SMTPImpostorHostAddedEvent(host.Settings.Id));

            host.Events.Subscribe(e => _events.OnNext(e));

            if (hostSettings.Start) host.Start();

            return host;
        }

        public ISMTPImpostorHost UpdateHost(Guid hostId, SMTPImpostorHostSettings hostSettings)
        {
            var host = Hosts[hostId];
            Hosts = Hosts.Remove(hostId);
            host.Dispose();

            host = CreateHost(hostSettings);

            Hosts = Hosts.Add(hostId, host);
            _events.OnNext(new SMTPImpostorHostUpdatedEvent(hostId));

            if (hostSettings.Start) host.Start();

            host.Events.Subscribe(e => _events.OnNext(e));

            return host;
        }

        public ISMTPImpostorHost TryRemoveHost(Guid hostId)
        {
            if (Hosts.TryGetValue(hostId, out var host))
            {
                host.Stop();
                Hosts = Hosts.Remove(hostId);
                _events.OnNext(new SMTPImpostorHostRemovedEvent(hostId));

                return host;
            }

            return null;
        }

        void IDisposable.Dispose()
        {
            if (Hosts != null)
            {
                foreach (var host in Hosts.ToArray())
                {
                    host.Value.Dispose();
                }
                Hosts = null;
            }
        }
    }
}
