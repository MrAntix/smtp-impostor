using System;

namespace SMTP.Impostor.Messages
{
    public abstract class SMTPImpostorMessageEventBase<TData> :
        SMTPImpostorMessageEventBase
    {
        public SMTPImpostorMessageEventBase(
            Guid hostId,
            string messageId,
            TData data) : base(hostId, messageId)
        {
            Data = data;
        }

        public TData Data { get; }
    }

    public abstract class SMTPImpostorMessageEventBase :
        ISMTPImpostorMessageEvent
    {
        public SMTPImpostorMessageEventBase(
            Guid hostId,
            string messageId)
        {
            Id = Guid.NewGuid();
            On = DateTimeOffset.UtcNow;
            HostId = hostId;
            MessageId = messageId;
        }

        public Guid Id { get; }
        public DateTimeOffset On { get; }
        public Guid HostId { get; }
        public string MessageId { get; }
    }

}
