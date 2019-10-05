using System;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Sockets
{
    public interface ISMTPImpostorSocketHandlerProvider
    {
        Task HandleAsync(
            ISMTPImpostorSocket socket,
            Action<SMTPImpostorMessage> onMessage);
    }
}
