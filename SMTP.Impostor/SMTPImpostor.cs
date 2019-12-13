using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor
{
    public class SMTPImpostor : ISMTPImpostorHostProvider, IDisposable
    {
        readonly ILogger<SMTPImpostor> _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly ISMTPImpostorSettings _settings;
        readonly IImmutableDictionary<string, ISMTPImpostorMessagesStoreProvider> _storeProviders;
        readonly Subject<ISMTPImpostorEvent> _events;

        public SMTPImpostor(
            ILogger<SMTPImpostor> logger,
            ILoggerFactory loggerFactory,
            ISMTPImpostorSettings settings,
            IEnumerable<ISMTPImpostorMessagesStoreProvider> storeProviders)
        {
            _logger = logger;
            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            _settings = settings;
            _events = new Subject<ISMTPImpostorEvent>();
            _storeProviders = storeProviders
                .ToDictionary(p => p.Type).ToImmutableDictionary();
        }

        public ISMTPImpostorHost CreateHost(
            SMTPImpostorHostSettings settings)
        {
            return new SMTPImpostorHost(
                _loggerFactory.CreateLogger<SMTPImpostorHost>(),
                settings,
                _storeProviders[settings.StoreType ?? _settings.DefaultStoreType].Create(settings.Id, settings.Store)
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

            host.Events.Subscribe(e => _events.OnNext(e));
            if (hostSettings.Start)
                Delegates.Retry(
                    () => host.Start(),
                    _logger);

            return host;
        }

        public ISMTPImpostorHost TryRemoveHost(Guid hostId)
        {
            if (Hosts.TryGetValue(hostId, out var host))
            {
                host.Dispose();
                Hosts = Hosts.Remove(hostId);
                _events.OnNext(new SMTPImpostorHostRemovedEvent(hostId));

                return host;
            }

            return null;
        }

        public void Shutdown()
        {
            if (Hosts != null)
            {
                _events.OnNext(new SMTPImpostorStoppedEvent());
                foreach (var host in Hosts.ToArray())
                {
                    host.Value.Dispose();
                }
                Hosts = null;
            }
        }

        void IDisposable.Dispose() => Shutdown();
    }
}
