using System;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageRemovedEvent :
        SMTPImpostorMessageEventBase
    {
        public SMTPImpostorMessageRemovedEvent(
            Guid hostId, string messageId) : base(hostId, messageId)
        {
        }
    }
}
