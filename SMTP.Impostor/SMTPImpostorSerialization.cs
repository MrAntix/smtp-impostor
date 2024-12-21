using System.Text.Json;

namespace SMTP.Impostor
{
    public class SMTPImpostorSerialization
    {
        readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, _options);
        }
    }
}
