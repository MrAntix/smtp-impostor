using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessagesStore
    {
        Task<SMTPImpostorMessage> GetAsync(Guid hostId, string messageId);
        Task PutAsync(Guid hostId, SMTPImpostorMessage message);
        Task<IImmutableList<SMTPImpostorMessage>> SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria);
    }
}
