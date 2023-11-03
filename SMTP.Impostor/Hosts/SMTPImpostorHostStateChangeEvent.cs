using System;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostStateChangeEvent :
         SMTPImpostorHostEventBase<SMTPImpostorHostStatus>
    {
        public SMTPImpostorHostStateChangeEvent(
            Guid hostId,
            SMTPImpostorHostStatus state
            ) : base(hostId, state)
        { }
    }
}
