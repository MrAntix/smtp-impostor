using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public abstract class HubActionBase<TRequest, TResponse> : IHubAction
    {
        Type IHubAction.RequestType => typeof(TRequest);

        public abstract Task<TResponse> ExecuteAsync(TRequest request);

        async Task<object> IHubAction.ExecuteAsync(object request)
        {
            return await ExecuteAsync((TRequest)request);
        }
    }
}
