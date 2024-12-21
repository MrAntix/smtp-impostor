using SMTP.Impostor.Messages;
using System;
using System.Threading.Tasks;

namespace SMTP.Impostor.Sockets
{
    public interface ISMTPImpostorSocketHandler
    {
        Task HandleAsync(Action<SMTPImpostorMessage> onMessage);
    }
}
