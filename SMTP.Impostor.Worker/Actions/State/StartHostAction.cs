using System.Linq;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class StartHostAction :
        VoidActionBase<HostIdentity>
    {
        readonly SMTPImpostor _impostor;

        public StartHostAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync(HostIdentity request)
        {
            var host = _impostor.Hosts
                .Values.First(h => h.Id == request.HostId);
            host.Start();

            return Task.CompletedTask;
        }
    }
}
