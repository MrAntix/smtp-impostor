using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public interface IHubActionExecutor
    {
        Task<object> ExecuteAsync(string type, string data);
    }
}
