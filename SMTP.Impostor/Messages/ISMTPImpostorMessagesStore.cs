using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Messages
{
    public interface ISMTPImpostorMessagesStore : IDisposable
    {
        int Count { get; }
        Task<SMTPImpostorMessage> GetAsync(string messageId);
        Task LaunchAsync(string messageId);
        Task PutAsync(SMTPImpostorMessage message);
        Task<SMTPImpostorMessageStoreSearchResult> SearchAsync(SMTPImpostorMessageStoreSearchCriteria criteria);
        Task DeleteAsync(string messageId);
        IObservable<ISMTPImpostorMessageEvent> Events { get; }
    }
}
