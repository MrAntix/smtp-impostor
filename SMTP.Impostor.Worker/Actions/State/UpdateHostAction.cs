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

        public override Task ExecuteAsync(HostUpdate update)
        {
            var host = _impostor.Hosts[update.HostId];

            var hostSettings = host.MapToSettings(update);
            _impostor.UpdateHost(update.HostId, hostSettings);

            return Task.CompletedTask;
        }
    }
}
