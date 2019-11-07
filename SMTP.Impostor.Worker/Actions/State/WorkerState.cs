using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class WorkerState
    {
        public WorkerState(
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
