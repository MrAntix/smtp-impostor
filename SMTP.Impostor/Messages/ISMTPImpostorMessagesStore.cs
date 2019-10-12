using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessagesStore
    {
        Task<SMTPImpostorMessage> GetAsync(string host, string messageId);
        Task PutAsync(string host, SMTPImpostorMessage message);
        Task<IImmutableList<SMTPImpostorMessage>> SearchAsync(SMTPImpostorStoreSearchCriteria criteria);
    }
}
