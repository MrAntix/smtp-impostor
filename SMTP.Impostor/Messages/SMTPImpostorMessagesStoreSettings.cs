namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessagesStoreSettings
    {
        public SMTPImpostorMessagesStoreSettings(
            int? maxMessages = 0)
        {
            MaxMessages = maxMessages.GetValueOrDefault();
        }

        public int MaxMessages { get; }

        public static readonly SMTPImpostorMessagesStoreSettings Default
            = new SMTPImpostorMessagesStoreSettings();
    }
}
