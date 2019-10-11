using System.Threading.Tasks;
using SMTP.Impostor.Store;
using SMTP.Impostor.Store.File;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public class GetStateAction :
        HubActionBase<GetState, State>
    {
        readonly SMTPImpostor _impostor;
        readonly ISMTPImpostorStore _store;

        public GetStateAction(
            SMTPImpostor impostor,
            ISMTPImpostorStore store)
        {
            _impostor = impostor;
            _store = store;
        }

        public override async Task<State> ExecuteAsync(
            GetState request)
        {
            var fileStore = _store as SMTPImpostorFileStore;
            if (fileStore == null)
                return new State(
                    _impostor.Hosts.Values.Map(),
                    null);

            return new State(
                _impostor.Hosts.Values.Map(),
                fileStore.StorePath);
        }
    }
}
