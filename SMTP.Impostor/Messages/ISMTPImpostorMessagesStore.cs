using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessagesStore
    {
        Task<SMTPImpostorMessage> GetAsync(string messageId);
        Task PutAsync(SMTPImpostorMessage message);
        Task<SMTPImpostorMessageStoreSearchResult> SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria);
    }
}
