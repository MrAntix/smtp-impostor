using SMTP.Impostor.Hosts;
using System;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostAddedEvent :
        SMTPImpostorHostEventBase
    {
        public SMTPImpostorHostAddedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
