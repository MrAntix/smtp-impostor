using SMTP.Impostor.Messages;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.Store
{
    public class SearchStoreAction :
        ActionBase<SMTPImpostorMessageStoreSearchCriteria, IImmutableList<SMTPImpostorMessageInfo>>
    {
        readonly SMTPImpostor _impostor;

        public SearchStoreAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override async Task<IImmutableList<SMTPImpostorMessageInfo>> ExecuteAsync(
            SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            var result = await _impostor
                .Hosts[criteria.HostId]
                .Messages
                .SearchAsync(criteria);

            return result;
        }
    }
}
