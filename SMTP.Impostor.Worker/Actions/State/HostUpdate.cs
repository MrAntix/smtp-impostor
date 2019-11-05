using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class HostUpdate
    {
        public HostUpdate(
            Guid hostId,
            string ip,
            int? port,
            string name)
        {
            HostId = hostId;
            IP = ip;
            Port = port;
            Name = name;
        }

        public Guid HostId { get; }
        public string IP { get; }
        public int? Port { get; }
        public string Name { get; }
    }
}
