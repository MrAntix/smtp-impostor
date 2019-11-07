using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostStateChangeEvent :
         SMTPImpostorEventBase<SMTPImpostorHostStatus>
    {
        public SMTPImpostorHostStateChangeEvent(
            Guid hostId,
            SMTPImpostorHostStatus state) : base(hostId, state)
        { }
    }
}
