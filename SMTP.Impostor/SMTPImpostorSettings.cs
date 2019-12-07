using System.IO;

namespace SMTP.Impostor
{
    public class SMTPImpostorSettings : ISMTPImpostorSettings
    {
        public string DefaultStoreType { get; set; } = "FileSystem";
        public string FileStoreRoot { get; set; } = Path.Combine(Path.GetTempPath(), "Impostor");

        public static ISMTPImpostorSettings Default { get; }
            = new SMTPImpostorSettings();
    }
}
