using System;
using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageReceivedEvent :
        SMTPImpostorEventBase<SMTPImpostorMessage>
    {
        public SMTPImpostorMessageReceivedEvent(
            Guid hostId,
            SMTPImpostorHostSettings hostSettings,
            SMTPImpostorMessage message) :
            base(hostId, message)
        {
            HostSettings = hostSettings;
        }

        public SMTPImpostorHostSettings HostSettings { get; }
    }
}
