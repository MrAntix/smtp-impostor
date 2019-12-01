using System;
using SMTP.Impostor.Hosts;

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
