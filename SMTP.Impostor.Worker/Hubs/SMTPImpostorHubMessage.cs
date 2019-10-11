using System;
using System.Text.Json;

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

        public static SMTPImpostorHubMessage From(object model)
        {
            return new SMTPImpostorHubMessage(
                model.GetType().Name,
                JsonSerializer.Serialize(model)
                );
        }
    }
}
