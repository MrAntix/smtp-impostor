using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public interface IHubAction
    {
        Type RequestType { get; }
        Task<object> ExecuteAsync(object request);
    }
}
