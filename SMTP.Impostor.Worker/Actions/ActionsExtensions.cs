using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions
{
    public static class ActionsExtensions
    {
        public static Task<object> ExecuteAsync(
            this IActionExecutor executor, string typeName)
        {
            return executor.ExecuteAsync(typeName, null);
        }
    }
}
