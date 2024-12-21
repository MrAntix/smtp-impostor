namespace SMTP.Impostor.Messages
{
    public record SMTPImpostorMessagesStoreSettings
    {
        public static readonly SMTPImpostorMessagesStoreSettings Default = new();

        public int MaxMessages { get; init; } = 0;
    }
}
