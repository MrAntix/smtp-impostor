using System.Collections.Immutable;
using System.Linq;
using SMTP.Impostor.Hosts;

namespace SMTP.Impostor.Worker.Actions.State
{
    public static class StatusExtensions
    {
        public static IImmutableList<SMTPImpostorHostSettings> ToSettings(
            this Status source)
        {
            return source.Hosts?.Select(h => h.ToSettings()).ToImmutableList();
        }

        public static SMTPImpostorHostSettings ToSettings(
            this HostStatus source)
        {
            if (source is null) return null;

            return new SMTPImpostorHostSettings(
                source.Id,
                source.IP, source.Port,
                source.Name,
                source.StoreType,
                source.State == Sockets.SMTPImpostorHostStates.Started);
        }
    }
}
