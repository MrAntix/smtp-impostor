using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageReceivedEvent :
        SMTPImpostorMessageEventBase<SMTPImpostorMessage>
    {
        public SMTPImpostorMessageReceivedEvent(
            SMTPImpostorHostSettings hostSettings,
            SMTPImpostorMessage message) :
            base(hostSettings.Id, message.Id, message)
        {
            HostSettings = hostSettings;
        }

        public SMTPImpostorHostSettings HostSettings { get; }
    }
}
