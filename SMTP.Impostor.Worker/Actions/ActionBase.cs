using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions
{
    public abstract class ActionBase
    {
        public static string GetName(Type actionType)
        {
            return actionType.Name[0..^6];
        }
    }

    public abstract class ActionBase<TRequest, TResponse> : ActionBase, IAction
    {
        Type IAction.RequestType => typeof(TRequest);

        public abstract Task<TResponse> ExecuteAsync(TRequest request);

        async Task<object> IAction.ExecuteAsync(object request)
        {
            return await ExecuteAsync((TRequest)request);
        }
    }

    public abstract class ActionBase<TResponse> : ActionBase, IAction
    {
        Type IAction.RequestType { get; } = null;

        public abstract Task<TResponse> ExecuteAsync();

        async Task<object> IAction.ExecuteAsync(object request)
        {
            return await ExecuteAsync();
        }
    }

    public abstract class VoidActionBase<TRequest> : ActionBase, IAction
    {
        Type IAction.RequestType => typeof(TRequest);

        public abstract Task ExecuteAsync(TRequest request);

        async Task<object> IAction.ExecuteAsync(object request)
        {
            await ExecuteAsync((TRequest)request);
            return ActionNull.Instance;
        }
    }

    public abstract class VoidActionBase : ActionBase, IAction
    {
        Type IAction.RequestType => typeof(ActionNull);

        public abstract Task ExecuteAsync();

        async Task<object> IAction.ExecuteAsync(object _)
        {
            await ExecuteAsync();
            return ActionNull.Instance;
        }
    }
}
