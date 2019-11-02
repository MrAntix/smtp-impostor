using System;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostStatus
    {
        public HostStatus(
            Guid id,
            string name,
            string ip, int port,
            string storeType,
            SMTPImpostorHostStates state)
        {
            Id = id;
            Name = name;
            IP = ip;
            Port = port;
            StoreType = storeType;
            State = state;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string IP { get; }
        public int Port { get; }
        public string StoreType { get; }
        public SMTPImpostorHostStates State { get; }
    }
}
