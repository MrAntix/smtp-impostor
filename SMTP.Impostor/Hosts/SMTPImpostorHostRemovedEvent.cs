using System;
using SMTP.Impostor.Hosts;

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
