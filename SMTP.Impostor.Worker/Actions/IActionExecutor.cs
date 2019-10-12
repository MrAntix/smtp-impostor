using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions
{
    public interface IActionExecutor
    {
        Task<object> ExecuteAsync(string type, string data);
    }
}
