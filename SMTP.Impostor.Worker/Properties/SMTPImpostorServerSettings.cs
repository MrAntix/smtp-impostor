namespace SMTP.Impostor.Worker.Properties
{
    public class SMTPImpostorServerSettings
    {
        public ISMTPImpostorSettings Impostor { get; set; }
        public SMTPImpostorWorkerSettings Worker { get; set; } = new SMTPImpostorWorkerSettings();
    }
}
