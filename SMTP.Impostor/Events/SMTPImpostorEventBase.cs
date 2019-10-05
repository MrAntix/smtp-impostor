using System;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Events
{
    public abstract class SMTPImpostorEventBase<TData> :
        ISMTPImpostorEvent
    {
        public SMTPImpostorEventBase(
            SMTPImpostorHostSettings hostSettings,
            TData data)
        {
            Id = Guid.NewGuid();
            On = DateTimeOffset.UtcNow;
            HostSettings = hostSettings;
            Data = data;
        }

        public Guid Id { get; }
        public DateTimeOffset On { get; }
        public SMTPImpostorHostSettings HostSettings { get; }
        public TData Data { get; }
    }
}
