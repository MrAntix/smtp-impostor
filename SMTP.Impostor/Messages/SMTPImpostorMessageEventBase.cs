using SMTP.Impostor.Hosts;
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
        SMTPImpostorHostEventBase, ISMTPImpostorMessageEvent
    {
        public SMTPImpostorMessageEventBase(
            Guid hostId,
            string messageId) : base(hostId)
        {
            MessageId = messageId;
        }

        public string MessageId { get; }
    }

}
