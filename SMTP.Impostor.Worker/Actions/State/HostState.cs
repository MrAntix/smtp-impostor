using System;
using SMTP.Impostor.Sockets;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostState
    {
        public HostState(
            Guid id,
            string name,
            string ip, int port,
            string storeType,
            SMTPImpostorHostStatus state)
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
        public SMTPImpostorHostStatus State { get; }
    }
}
