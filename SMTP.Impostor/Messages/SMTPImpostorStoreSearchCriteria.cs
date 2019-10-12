using System.Collections.Generic;
using System.Collections.Immutable;

namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorStoreSearchCriteria
    {
        public SMTPImpostorStoreSearchCriteria(
            string host,
            IEnumerable<string> ids,
            string text,
            int index = 0,
            int count = 50
            )
        {
            Ids = ids?.ToImmutableList();
            Host = host;
            Text = text;
            Index = index;
            Count = count;
        }

        public IImmutableList<string> Ids { get; }
        public string Host { get; }
        public string Text { get; }
        public int Index { get; }
        public int Count { get; }
    }
}
