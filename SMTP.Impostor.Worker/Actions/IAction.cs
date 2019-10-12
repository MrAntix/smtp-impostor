using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Actions
{
    public interface IAction
    {
        Type RequestType { get; }
        Task<object> ExecuteAsync(object request);
    }
}
