using SMTP.Impostor.Hosts;
using System;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostRemovedEvent :
        SMTPImpostorHostEventBase
    {
        public SMTPImpostorHostRemovedEvent(
            Guid hostId) : base(hostId)
        {
        }
    }
}
