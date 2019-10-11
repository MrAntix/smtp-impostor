using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Worker.Hubs.Actions
{
    public static class StateMapper
    {
        public static IImmutableList<HostState> Map(
            this IEnumerable<ISMTPImpostorHost> source)
        {
            return source?.Select(Map).ToImmutableList();
        }

        public static HostState Map(
            this ISMTPImpostorHost source)
        {
            if (source == null) return null;

            return new HostState(
                source.Settings.Name,
                source.Settings.IP, source.Settings.Port,
                source.State
                );
        }
    }
}
