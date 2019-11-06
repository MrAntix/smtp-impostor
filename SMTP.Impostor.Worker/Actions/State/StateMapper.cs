using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SMTP.Impostor.Hosts;
using SMTP.Impostor.Sockets;

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
                source.Settings.Id,
                source.Settings.Name,
                source.Settings.IP, source.Settings.Port,
                0,
                source.Settings.StoreType,
                source.State
                );
        }

        public static SMTPImpostorHostSettings MapToSettings(
            this ISMTPImpostorHost source,
            HostUpdate update)
        {
            if (source == null) return null;

            return new SMTPImpostorHostSettings(
                source.Settings.Id,
                update.IP ?? source.Settings.IP, update.Port ?? source.Settings.Port,
                update.Name ?? source.Settings.Name,
                source.Settings.StoreType,
                source.State == SMTPImpostorHostStates.Started
                );
        }
    }
}
