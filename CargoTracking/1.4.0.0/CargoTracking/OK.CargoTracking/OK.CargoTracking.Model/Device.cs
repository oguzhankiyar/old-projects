using Newtonsoft.Json;

namespace OK.CargoTracking.Model
{
    public class Device
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "appVersion")]
        public string AppVersion { get; set; }
    }
}
