using System;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessagesStoreProvider
    {
        string Type { get; }
        ISMTPImpostorMessagesStore Create(Guid hostId, SMTPImpostorMessagesStoreSettings settings);
    }
}
