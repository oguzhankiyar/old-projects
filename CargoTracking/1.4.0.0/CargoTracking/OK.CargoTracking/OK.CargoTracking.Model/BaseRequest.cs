using Newtonsoft.Json;

namespace OK.CargoTracking.Model
{
    public class BaseRequest
    {
        [JsonProperty(PropertyName = "device")]
        public Device Device { get; set; }

        [JsonProperty(PropertyName = "request")]
        public object Request { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}