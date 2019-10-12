using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions
{
    public abstract class ActionBase<TRequest, TResponse> : IAction
    {
        Type IAction.RequestType => typeof(TRequest);

        public abstract Task<TResponse> ExecuteAsync(TRequest request);

        async Task<object> IAction.ExecuteAsync(object request)
        {
            return await ExecuteAsync((TRequest)request);
        }
    }
}
