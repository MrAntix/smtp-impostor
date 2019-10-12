using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Worker.Actions.State
{
    public static class StateMapper
    {
        public static IImmutableList<HostStatus> Map(
            this IEnumerable<ISMTPImpostorHost> source)
        {
            return source?.Select(Map).ToImmutableList();
        }

        public static HostStatus Map(
            this ISMTPImpostorHost source)
        {
            if (source == null) return null;

            return new HostStatus(
                source.Id,
                source.Settings.Name,
                source.Settings.IP, source.Settings.Port,
                source.State
                );
        }
    }
}
