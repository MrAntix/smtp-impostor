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

        public static async Task<TResponse> ExecuteAsync<TResponse>(
            this IActionExecutor executor, string typeName)
            where TResponse : class
        {
            return (await executor.ExecuteAsync(typeName, null)) as TResponse;
        }

        public static async Task<TResponse> ExecuteAsync<TResponse>(
            this IActionExecutor executor, string typeName, string request)
            where TResponse : class
        {
            return (await executor.ExecuteAsync(typeName, request)) as TResponse;
        }

        public static async Task<TResponse> ExecuteAsync<TAction, TResponse>(
            this IActionExecutor executor)
            where TResponse : class
        {
            var typeName = ActionBase.GetName(typeof(TAction));

            return (await executor.ExecuteAsync(typeName)) as TResponse;
        }
    }
}

