using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostUpdatedEvent :
        SMTPImpostorEventBase
    {
        public SMTPImpostorHostUpdatedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
