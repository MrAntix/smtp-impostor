using System.Threading.Tasks;
using SMTP.Impostor.Worker.Actions;

namespace SMTP.Impostor.Test
{
    internal class FakeActionExecutor : IActionExecutor
    {
        Task<object> IActionExecutor
            .ExecuteAsync(string type, string data)
        {
            return Task.FromResult((object)NullActionResponse.Instance);
        }
    }
}
