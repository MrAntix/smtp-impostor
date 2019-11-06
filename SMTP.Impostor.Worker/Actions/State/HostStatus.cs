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
            int messagesCount,
            string storeType,
            SMTPImpostorHostStates state)
        {
            Id = id;
            Name = name;
            IP = ip;
            Port = port;
            MessagesCount = messagesCount;
            StoreType = storeType;
            State = state;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string IP { get; }
        public int Port { get; }
        public int MessagesCount { get; }
        public string StoreType { get; }
        public SMTPImpostorHostStates State { get; }
    }
}
