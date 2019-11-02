﻿using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageReceivedEvent :
        SMTPImpostorEventBase<SMTPImpostorMessage>
    {
        public SMTPImpostorMessageReceivedEvent(
            SMTPImpostorHostSettings hostSettings,
            SMTPImpostorMessage message) :
            base(hostSettings.Id, message)
        {
            HostSettings = hostSettings;
        }

        public SMTPImpostorHostSettings HostSettings { get; }
    }
}
