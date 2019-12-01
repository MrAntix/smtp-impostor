using System;

namespace SMTP.Impostor.Events
{
    public abstract class SMTPImpostorEventBase :
        ISMTPImpostorEvent
    {
        public SMTPImpostorEventBase()
        {
            Id = Guid.NewGuid();
            On = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; }
        public DateTimeOffset On { get; }
    }

    public abstract class SMTPImpostorEventBase<TData> :
        SMTPImpostorEventBase
    {
        public SMTPImpostorEventBase(
            TData data) : base()
        {
            Data = data;
        }

        public TData Data { get; }
    }
}
