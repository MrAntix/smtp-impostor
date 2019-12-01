using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class ShutdownWorkerAction : VoidActionBase
    {
        readonly SMTPImpostor _impostor;

        public ShutdownWorkerAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync()
        {
            _impostor.Shutdown();

            return Task.CompletedTask;
        }
    }
}
