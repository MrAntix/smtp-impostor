using SMTP.Impostor.Events;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessageEvent : ISMTPImpostorHostEvent
    {
        string MessageId { get; }
    }
}
