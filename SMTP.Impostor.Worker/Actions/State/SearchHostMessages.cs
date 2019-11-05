using SMTP.Impostor.Messages;
using System;

namespace SMTP.Impostor.Worker.Actions.State
{
    public class SearchHostMessages
    {
        public SearchHostMessages(
            Guid hostId,
            SMTPImpostorMessageStoreSearchCriteria criteria)
        {
            HostId = hostId;
            Criteria = criteria;
        }

        public Guid HostId { get; }
        public SMTPImpostorMessageStoreSearchCriteria Criteria { get; }
    }
}
