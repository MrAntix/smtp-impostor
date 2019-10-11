using System.Collections.Immutable;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Store;

namespace SMTP.Impostor.Worker.Hubs.Actions
{

    public class SearchStoreAction :
        HubActionBase<SMTPImpostorStoreSearchCriteria, IImmutableList<SMTPImpostorMessage>>
    {
        readonly ISMTPImpostorStore _store;

        public SearchStoreAction(
            ISMTPImpostorStore store)
        {
            _store = store;
        }

        public override async Task<IImmutableList<SMTPImpostorMessage>> ExecuteAsync(
            SMTPImpostorStoreSearchCriteria request)
        {
            var result = await _store
                .SearchAsync(request);

            return result;
        }
    }
}
