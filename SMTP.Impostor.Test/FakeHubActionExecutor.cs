using System.Threading.Tasks;
using SMTP.Impostor.Worker.Hubs.Actions;

namespace SMTP.Impostor.Test
{
    internal class FakeHubActionExecutor : IHubActionExecutor
    {
        Task<object> IHubActionExecutor
            .ExecuteAsync(string type, string data)
        {
            return Task.FromResult((object)NullActionResponse.Instance);
        }
    }
}
