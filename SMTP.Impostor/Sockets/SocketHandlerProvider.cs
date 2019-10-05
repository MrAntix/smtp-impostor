using System;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Sockets
{
    internal class SocketHandlerProvider : ISMTPImpostorSocketHandlerProvider
    {
        Task ISMTPImpostorSocketHandlerProvider
            .HandleAsync(
                ISMTPImpostorSocket socket,
                Action<SMTPImpostorMessage> onMessage)
        {
            var handler = new SocketHandler(socket, onMessage);
            return handler.HandleAsync();
        }
    }
}
