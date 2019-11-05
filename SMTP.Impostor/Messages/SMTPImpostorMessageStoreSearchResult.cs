using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageStoreSearchResult
    {
        public SMTPImpostorMessageStoreSearchResult(
            int index,
            int total,
            IEnumerable<SMTPImpostorMessageInfo> messages
            )
        {
            Messages = messages.ToImmutableList();
            Index = index;
            Total = total;
        }

        public IImmutableList<SMTPImpostorMessageInfo> Messages { get; }
        public int Index { get; }
        public int Total { get; }

        public readonly static SMTPImpostorMessageStoreSearchResult Empty
            = new SMTPImpostorMessageStoreSearchResult(
                0, 0,
                Enumerable.Empty<SMTPImpostorMessageInfo>());
    }
}
