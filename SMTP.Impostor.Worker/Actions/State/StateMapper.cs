using SMTP.Impostor.Hosts;
using SMTP.Impostor.Messages;
using SMTP.Impostor.Sockets;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SMTP.Impostor.Worker.Actions.State
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
                source.Settings.Id,
                source.Settings.Name,
                source.Settings.IP, source.Settings.Port,
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
                source.State == SMTPImpostorHostStatus.Started
                );
        }

        public static IImmutableList<MessageInfo> Map(
            this IEnumerable<SMTPImpostorMessageInfo> source)
        {
            return source?.Select(Map).ToImmutableList();
        }

        public static MessageInfo Map(
            this SMTPImpostorMessageInfo source)
        {
            if (source == null) return null;

            return new MessageInfo(
                source.Id,
                source.Date,
                source.From.ToString(),
                source.Subject
                );
        }
    }
}
