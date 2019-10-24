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

        public override Task ExecuteAsync(SMTPImpostorHostSettings settings)
        {
            _impostor.AddHost(settings);

            return Task.CompletedTask;
        }
    }
}
