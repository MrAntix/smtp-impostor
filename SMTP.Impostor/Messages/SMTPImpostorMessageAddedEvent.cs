using System;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageAddedEvent :
        SMTPImpostorMessageEventBase
    {
        public SMTPImpostorMessageAddedEvent(
            Guid hostId, string messageId) : base(hostId, messageId)
        {
        }
    }
}
