using SMTP.Impostor.Hosts;
using System;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostUpdatedEvent :
        SMTPImpostorHostEventBase
    {
        public SMTPImpostorHostUpdatedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
