using SMTP.Impostor.Store.File;

namespace SMTP.Impostor.Worker.Properties
{
    public class SMTPImpostorServerSettings
    {
        public ISMTPImpostorSettings Impostor { get; set; }
        public SMTPImpostorFileStoreSettings ImpostorFileStore { get; set; }
    }
}
