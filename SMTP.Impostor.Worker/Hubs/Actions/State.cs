using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public class State
    {
        public State(
            IEnumerable<HostState> hosts,
            string fileStorePath)
        {
            Hosts = hosts?.ToImmutableList();
            FileStorePath = fileStorePath;
        }

        public IImmutableList<HostState> Hosts { get; }
        public string FileStorePath { get; }
    }
}
