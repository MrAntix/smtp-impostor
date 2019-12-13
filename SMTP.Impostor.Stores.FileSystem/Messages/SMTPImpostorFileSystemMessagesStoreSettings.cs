using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Stores.FileSystem.Messages
{
    public class SMTPImpostorFileSystemMessagesStoreSettings
    {
        public SMTPImpostorFileSystemMessagesStoreSettings(
            SMTPImpostorMessagesStoreSettings general,
            string fileStoreRoot)
        {
            General = general;
            FileStoreRoot = fileStoreRoot;
        }

        public SMTPImpostorMessagesStoreSettings General { get; }
        public string FileStoreRoot { get; }
    }
}
