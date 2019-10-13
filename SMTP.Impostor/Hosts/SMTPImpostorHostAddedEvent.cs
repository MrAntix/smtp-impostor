using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostAddedEvent :
        SMTPImpostorEventBase
    {
        public SMTPImpostorHostAddedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
