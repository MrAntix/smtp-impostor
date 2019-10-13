using System;
using SMTP.Impostor.Events;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostStateChangeEvent :
         SMTPImpostorEventBase<SMTPImpostorHostStates>
    {
        public SMTPImpostorHostStateChangeEvent(
            Guid hostId,
            SMTPImpostorHostStates state) : base(hostId, state)
        { }
    }
}
