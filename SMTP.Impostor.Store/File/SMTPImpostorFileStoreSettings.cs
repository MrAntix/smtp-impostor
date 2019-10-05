namespace SMTP.Impostor.Store.File
{
    public class SMTPImpostorFileStoreSettings
    {
        public string Path { get; set; }

        public static SMTPImpostorFileStoreSettings Default { get; }
            = new SMTPImpostorFileStoreSettings();
    }
}
