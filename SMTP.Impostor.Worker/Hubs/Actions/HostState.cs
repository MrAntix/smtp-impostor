using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public class HostState
    {
        public HostState(
            string name,
            string ip, int port,
            SMTPImpostorHostStates state)
        {
            Name = name;
            IP = ip;
            Port = port;
            State = state;
        }

        public string Name { get; }
        public string IP { get; }
        public int Port { get; }
        public SMTPImpostorHostStates State { get; }
    }
}
