using System;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Sockets
{
    public interface ISMTPImpostorSocketHandler :
        IDisposable
    {
        Task<SMTPImpostorMessage> HandleAsync();
    }
}
