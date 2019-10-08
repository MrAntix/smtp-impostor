namespace SMTP.Impostor.Worker.Hubs
{
    public class SMTPImpostorHubMessage
    {
        public SMTPImpostorHubMessage(
            string type,
            string data)
        {
            Type = type;
            Data = data;
        }

        public string Type { get; }
        public string Data { get; }
    }
}
