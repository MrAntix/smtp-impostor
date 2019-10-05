using System;
using System.Net;
using System.Net.Sockets;

namespace SMTP.Impostor.Sockets
{
    public interface ISMTPImpostorSocket : IDisposable
    {
        IPEndPoint RemoteEndPoint { get; }
        NetworkStream GetNetworkStream();
    }
}
