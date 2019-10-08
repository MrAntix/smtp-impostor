using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs
{
    public class SMTPImpostorHubClient :
        ISMTPImpostorHubClient
    {
        readonly WebSocket _webSocket;

        public SMTPImpostorHubClient(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        WebSocketState ISMTPImpostorHubClient.State => _webSocket.State;

        async Task<string> ISMTPImpostorHubClient.ReceiveAsync()
        {
            var _buffer = new ArraySegment<byte>(new byte[1024 * 4]);

            using var data = new MemoryStream();

            var result = default(WebSocketReceiveResult);
            do
            {
                result = await _webSocket
                    .ReceiveAsync(_buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close
                    || result.MessageType != WebSocketMessageType.Text)
                    return null;

                await data.WriteAsync(_buffer.Array, 0, result.Count);

            } while (!result.EndOfMessage);

            return Encoding.UTF8.GetString(data.ToArray());
        }

        async Task ISMTPImpostorHubClient.SendAsync(
            string data)
        {
            var bytes = new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes(data)
                    );

            await _webSocket.SendAsync(
                bytes,
                WebSocketMessageType.Text, true,
                CancellationToken.None);
        }

        async Task ISMTPImpostorHubClient.CloseAsync()
        {
            await _webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure, "NormalClosure",
                CancellationToken.None);
        }

        public static ISMTPImpostorHubClient Wrap(WebSocket webSocket)
        {
            if (webSocket is null)
                throw new ArgumentNullException(nameof(webSocket));

            return new SMTPImpostorHubClient(webSocket);
        }
    }
}
