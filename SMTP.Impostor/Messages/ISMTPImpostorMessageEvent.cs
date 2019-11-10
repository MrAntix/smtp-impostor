using SMTP.Impostor.Events;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessageEvent : ISMTPImpostorEvent
    {
        string MessageId { get; }
    }
}
