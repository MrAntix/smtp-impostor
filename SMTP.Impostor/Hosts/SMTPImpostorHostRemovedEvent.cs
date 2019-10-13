using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostRemovedEvent :
        SMTPImpostorEventBase
    {
        public SMTPImpostorHostRemovedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
