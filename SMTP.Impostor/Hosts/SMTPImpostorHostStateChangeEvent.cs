using SMTP.Impostor.Hosts;
using System;

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
