using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class GetStatusAction :
        ActionBase<Status>
    {
        public static string Name { get; } = GetName(typeof(GetStatusAction));

        readonly ISMTPImpostorSettings _settings;
        readonly SMTPImpostor _impostor;

        public GetStatusAction(
            ISMTPImpostorSettings settings,
            SMTPImpostor impostor)
        {
            _settings = settings;
            _impostor = impostor;
        }

        public override Task<Status> ExecuteAsync()
        {
            var status = new Status(
                    _impostor.Hosts.Values.Map(),
                    _settings.FileStoreRoot);

            return Task.FromResult(status);
        }
    }
}
