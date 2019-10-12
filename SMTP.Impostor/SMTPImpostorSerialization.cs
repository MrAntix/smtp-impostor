using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SMTP.Impostor
{
    public class SMTPImpostorSerialization
    {
        readonly JsonSerializerSettings _settings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
