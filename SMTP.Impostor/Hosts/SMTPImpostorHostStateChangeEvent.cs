using SMTP.Impostor.Events;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Sockets
{
    public class SMTPImpostorHostStateChangeEvent :
         SMTPImpostorEventBase<SMTPImpostorHostStates>
    {
        public SMTPImpostorHostStateChangeEvent(
            SMTPImpostorHostSettings hostSettings,
            SMTPImpostorHostStates state) :
            base(hostSettings, state)
        { }
    }
}
