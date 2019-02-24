using Newtonsoft.Json;

namespace OK.CargoTracking.Model
{
    public class TrackingSearch : IEntityBase
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "factoryId")]
        public int FactoryId { get; set; }
    }
}
