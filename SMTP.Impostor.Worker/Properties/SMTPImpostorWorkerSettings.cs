namespace SMTP.Impostor.Worker.Properties
{
    public class SMTPImpostorWorkerSettings :
        SMTPImpostorSettings, ISMTPImpostorWorkerSettings
    {
        public string StartupMessageLink { get; set; }
    }
}
