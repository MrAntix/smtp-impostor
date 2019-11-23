namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessagesStoreSettings
    {
        public SMTPImpostorMessagesStoreSettings(
            int? maxMessages = null)
        {
            MaxMessages = maxMessages;
        }

        public int? MaxMessages { get; }

        public static readonly SMTPImpostorMessagesStoreSettings Default
            = new SMTPImpostorMessagesStoreSettings();
    }
}
