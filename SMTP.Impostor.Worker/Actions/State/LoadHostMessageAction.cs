using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class LoadHostMessageAction:
        VoidActionBase<HostMessageIdentity>
    {
        public static string Name { get; } = GetName(typeof(DeleteHostMessageAction));

        readonly SMTPImpostor _impostor;

        public LoadHostMessageAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override async Task ExecuteAsync(
            HostMessageIdentity request)
        {
            var host = _impostor.Hosts[request.HostId];

            await host.Messages.LaunchAsync(request.MessageId);
        }
    }
}
