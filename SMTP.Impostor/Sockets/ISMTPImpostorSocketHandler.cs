using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Sockets
{
    public interface ISMTPImpostorSocketHandler :
        IDisposable
    {
        Task HandleAsync();
    }
}
