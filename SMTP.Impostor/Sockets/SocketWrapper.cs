using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SMTP.Impostor.Sockets
{
    internal class SocketWrapper :
        ISMTPImpostorSocket
    {
        readonly Socket _socket;

        SocketWrapper(
            Socket socket)
        {
            _socket = socket;
        }

        IPEndPoint ISMTPImpostorSocket.RemoteEndPoint => (IPEndPoint)_socket.RemoteEndPoint;

        NetworkStream ISMTPImpostorSocket.GetNetworkStream()
        {
            return new NetworkStream(_socket, FileAccess.ReadWrite, true);
        }

        void IDisposable.Dispose()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _socket.Dispose();
        }

        public static ISMTPImpostorSocket Wrap(Socket socket)
        {
            return new SocketWrapper(socket);
        }
    }
}
