using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageStoreSearchCriteria
    {
        public SMTPImpostorMessageStoreSearchCriteria(
            Guid hostId,
            IEnumerable<string> ids,
            string text,
            int index = 0,
            int count = 50
            )
        {
            Ids = ids?.ToImmutableList();
            HostId = hostId;
            Text = text;
            Index = index;
            Count = count;
        }

        public Guid HostId { get; }
        public IImmutableList<string> Ids { get; }
        public string Text { get; }
        public int Index { get; }
        public int Count { get; }
    }
}
