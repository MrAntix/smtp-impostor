using System;

namespace SMTP.Impostor.Events
{
    public abstract class SMTPImpostorEventBase :
        ISMTPImpostorEvent
    {
        public SMTPImpostorEventBase(
            Guid hostId)
        {
            Id = Guid.NewGuid();
            On = DateTimeOffset.UtcNow;
            HostId = hostId;
        }

        public Guid Id { get; }
        public DateTimeOffset On { get; }
        public Guid HostId { get; }
    }

    public abstract class SMTPImpostorEventBase<TData> :
        SMTPImpostorEventBase
    {
        public SMTPImpostorEventBase(
            Guid hostId,
            TData data) : base(hostId)
        {
            Data = data;
        }

        public TData Data { get; }
    }
}
