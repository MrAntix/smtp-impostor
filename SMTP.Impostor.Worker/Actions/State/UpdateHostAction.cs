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
            var host = _impostor.Hosts[update.Id];

            var hostSetting = host.MapToSettings(update);
            _impostor.UpdateHost(host.Id, hostSetting);

            return Task.CompletedTask;
        }
    }
}
