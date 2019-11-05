using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class SearchHostMessagesAction :
        ActionBase<SearchHostMessages, HostMessagesLoaded>
    {

        public static string Name { get; } = GetName(typeof(SearchHostMessagesAction));

        readonly SMTPImpostor _impostor;

        public SearchHostMessagesAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override async Task<HostMessagesLoaded> ExecuteAsync(
            SearchHostMessages model)
        {
            var host = _impostor.Hosts[model.HostId];
            var result = await host.Messages.SearchAsync(model.Criteria);

            return new HostMessagesLoaded(
                model.HostId,
                result);
        }
    }
}
