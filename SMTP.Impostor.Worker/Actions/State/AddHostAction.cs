using System.Threading.Tasks;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class AddHostAction :
        VoidActionBase<SMTPImpostorHostSettings>
    {
        readonly SMTPImpostor _impostor;

        public AddHostAction(
            SMTPImpostor impostor)
        {
            _impostor = impostor;
        }

        public override Task ExecuteAsync(SMTPImpostorHostSettings request)
        {
            _impostor.AddHost(request);

            return Task.CompletedTask;
        }
    }
}
