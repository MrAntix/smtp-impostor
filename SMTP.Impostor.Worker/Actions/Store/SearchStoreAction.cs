using System.Collections.Immutable;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Worker.Actions.Store
{

    public class SearchStoreAction :
        ActionBase<SMTPImpostorMessageStoreSearchCriteria, IImmutableList<SMTPImpostorMessage>>
    {
        readonly ISMTPImpostorMessagesStore _store;

        public SearchStoreAction(
            ISMTPImpostorMessagesStore store)
        {
            _store = store;
        }

        public override async Task<IImmutableList<SMTPImpostorMessage>> ExecuteAsync(
            SMTPImpostorMessageStoreSearchCriteria request)
        {
            var result = await _store
                .SearchAsync(request);

            return result;
        }
    }
}
