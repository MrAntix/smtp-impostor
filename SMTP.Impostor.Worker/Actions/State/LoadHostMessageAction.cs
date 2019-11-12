using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class LoadHostMessageAction:
        ActionBase<HostMessageIdentity, HostMessage>
    {
        public static string Name { get; } = GetName(typeof(DeleteHostMessageAction));

        readonly SMTPImpostor _impostor;

        public LoadHostMessageAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override async Task<HostMessage> ExecuteAsync(
            HostMessageIdentity request)
        {
            var host = _impostor.Hosts[request.HostId];

            var message = await host.Messages.GetAsync(request.MessageId);

            return message.Map();
        }
    }
}
