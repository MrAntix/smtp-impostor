using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class LoadWorkerStateAction :
        ActionBase<WorkerState>
    {
        public static string Name { get; } = GetName(typeof(LoadWorkerStateAction));

        readonly ISMTPImpostorSettings _settings;
        readonly SMTPImpostor _impostor;

        public LoadWorkerStateAction(
            ISMTPImpostorSettings settings,
            SMTPImpostor impostor)
        {
            _settings = settings;
            _impostor = impostor;
        }

        public override Task<WorkerState> ExecuteAsync()
        {
            var status = new WorkerState(
                    _impostor.Hosts.Values.Map(),
                    _settings.FileStoreRoot);

            return Task.FromResult(status);
        }
    }
}
