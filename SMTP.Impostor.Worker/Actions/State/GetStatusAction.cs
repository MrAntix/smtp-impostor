using System.Threading.Tasks;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Stores.FileSystem.Messages;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class GetStatusAction :
        ActionBase<GetStatus, Status>
    {
        readonly SMTPImpostor _impostor;
        readonly ISMTPImpostorMessagesStore _store;

        public GetStatusAction(
            SMTPImpostor impostor,
            ISMTPImpostorMessagesStore store)
        {
            _impostor = impostor;
            _store = store;
        }

        public override Task<Status> ExecuteAsync(
            GetStatus request)
        {
            var fileStore = _store as SMTPImpostorFileSystemMessagesStore;
            var status = new Status(
                    _impostor.Hosts.Values.Map(),
                     fileStore?.StorePath);

            return Task.FromResult(status);
        }
    }
}
