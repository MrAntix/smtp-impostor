using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SMTP.Impostor.Worker.Hubs
{
    public interface ISMTPImpostorHubClient
    {
        WebSocketState State { get; }
        Task<string> ReceiveAsync();
        Task SendAsync(string data);
        Task CloseAsync();
    }
}
