using SMTP.Impostor.Hosts;
using System.Collections.Immutable;
using System.Linq;

namespace SMTP.Impostor.Worker.Actions.State
{
    public static class StateExtensions
    {
        public static IImmutableList<SMTPImpostorHostSettings> ToSettings(
            this WorkerState source)
        {
            return source.Hosts?.Select(h => h.ToSettings()).ToImmutableList();
        }

        public static SMTPImpostorHostSettings ToSettings(
            this HostState source)
        {
            if (source is null) return null;

            return new()
            {
                Id = source.Id,
                IP = source.IP,
                Port = source.Port,
                Name = source.Name,
                StoreType = source.StoreType,
                Start = source.State == Sockets.SMTPImpostorHostStatus.Started,
                Store = new()
                {
                    MaxMessages = source.MaxMessages
                }
            };
        }
    }
}
