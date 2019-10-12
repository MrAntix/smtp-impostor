using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class Status
    {
        public Status(
            IEnumerable<HostStatus> hosts,
            string fileStorePath)
        {
            Hosts = hosts?.ToImmutableList();
            FileStorePath = fileStorePath;
        }

        public IImmutableList<HostStatus> Hosts { get; }
        public string FileStorePath { get; }
    }
}
