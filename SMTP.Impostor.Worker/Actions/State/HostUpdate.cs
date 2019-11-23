using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostUpdate
    {
        public HostUpdate(
            Guid id,
            string ip, int? port,
            string name,
            int? maxMessages)
        {
            Id = id;
            IP = ip;
            Port = port;
            Name = name;
            MaxMessages = maxMessages;
        }

        public Guid Id { get; }
        public string IP { get; }
        public int? Port { get; }
        public string Name { get; }
        public int? MaxMessages { get; }
    }
}
