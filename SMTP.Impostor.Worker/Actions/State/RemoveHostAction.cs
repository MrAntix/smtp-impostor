using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class RemoveHostAction :
        VoidActionBase<HostIdentity>
    {
        readonly SMTPImpostor _impostor;

        public RemoveHostAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync(HostIdentity request)
        {
            var host = _impostor.Hosts
                .Values.First(h => h.Id == request.HostId);
            _impostor.RemoveHost(host.Settings);

            return Task.CompletedTask;
        }
    }
}
