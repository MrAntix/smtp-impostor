using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Store
{
    public interface ISMTPImpostorStore
    {
        Task<SMTPImpostorMessage> GetAsync(string host, string messageId);
        Task PutAsync(string host, SMTPImpostorMessage message);
        Task SearchAsync(SMTPImpostorStoreSearchCriteria criteria);
    }
}
