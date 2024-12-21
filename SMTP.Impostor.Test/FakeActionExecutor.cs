using SMTP.Impostor.Worker.Actions;
using System.Threading.Tasks;

namespace SMTP.Impostor.Test
{
    internal class FakeActionExecutor : IActionExecutor
    {
        Task<object> IActionExecutor
            .ExecuteAsync(string type, string data)
        {
            return Task.FromResult((object)ActionNull.Instance);
        }
    }

    internal class FakeAction : VoidActionBase
    {
        public static string NAME { get; } = GetName(typeof(FakeAction));

        public override Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }
    }
}
