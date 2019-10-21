﻿using System.Collections.Generic;
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
                source.Id,
                source.Settings.Name,
                source.Settings.IP, source.Settings.Port,
                source.State
                );
        }

        public static SMTPImpostorHostSettings MapToSettings(
            this ISMTPImpostorHost source,
            HostUpdate update)
        {
            if (source == null) return null;

            return new SMTPImpostorHostSettings(
                update.IP ?? source.Settings.IP, update.Port ?? source.Settings.Port,
                update.Name ?? source.Settings.Name,
                source.State == SMTPImpostorHostStates.Started
                );
        }
    }
}
