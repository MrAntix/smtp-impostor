using System.Collections.Immutable;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Worker.Actions.Store
{

    public class SearchStoreAction :
        ActionBase<SMTPImpostorStoreSearchCriteria, IImmutableList<SMTPImpostorMessage>>
    {
        readonly ISMTPImpostorMessagesStore _store;

        public SearchStoreAction(
            ISMTPImpostorMessagesStore store)
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
