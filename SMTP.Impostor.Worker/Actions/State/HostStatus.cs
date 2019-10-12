using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostStatus
    {
        public HostStatus(
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
