using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class UpdateHostAction :
        VoidActionBase<HostUpdate>
    {
        public static string Name { get; } = GetName(typeof(UpdateHostAction));

        readonly SMTPImpostor _impostor;

        public UpdateHostAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync(HostUpdate request)
        {
            var host = _impostor.Hosts[request.Id];

            var hostSettings = host.MapToSettings(request);
            _impostor.UpdateHost(request.Id, hostSettings);

            return Task.CompletedTask;
        }
    }
}
