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
                source.State,
                source.Messages.Count,
                source.Settings.Store.MaxMessages
                );
        }

        public static SMTPImpostorHostSettings MapToSettings(
            this ISMTPImpostorHost source,
            HostUpdate update)
        {
            if (source == null) return null;

            return new()
            {
                Id = source.Settings.Id,
                IP = update.IP ?? source.Settings.IP,
                Port = update.Port ?? source.Settings.Port,
                Name = update.Name ?? source.Settings.ToString(),
                StoreType = source.Settings.StoreType,
                Start = source.State == SMTPImpostorHostStatus.Started,
                Store = new()
                {
                    MaxMessages = update.MaxMessages ?? source.Settings.Store.MaxMessages
                }
            };
        }

        public static IImmutableList<HostMessageInfo> Map(
            this IEnumerable<SMTPImpostorMessageInfo> source)
        {
            return source?.Select(Map).ToImmutableList();
        }

        public static HostMessageInfo Map(
            this SMTPImpostorMessageInfo source)
        {
            if (source == null) return null;

            return new HostMessageInfo(
                source.Id,
                source.Date,
                source.From.ToString(),
                source.Subject
                );
        }

        public static HostMessage Map(
            this SMTPImpostorMessage source)
        {
            if (source == null) return null;

            return new HostMessage(
                source.Id,
                source.Date,
                source.From.ToString(),
                source.Subject,
                source.Content
                );
        }
    }
}
