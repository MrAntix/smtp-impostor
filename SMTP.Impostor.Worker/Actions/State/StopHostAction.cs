using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class StopHostAction :
        VoidActionBase<HostIdentity>
    {
        readonly SMTPImpostor _impostor;

        public StopHostAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync(HostIdentity request)
        {
            var host = _impostor.Hosts
                .Values.First(h => h.Settings.Id == request.HostId);
            host.Stop();

            return Task.CompletedTask;
        }
    }
}
