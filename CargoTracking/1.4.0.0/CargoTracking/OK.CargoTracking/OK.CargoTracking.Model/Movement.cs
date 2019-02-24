using Newtonsoft.Json;
using System;

namespace OK.CargoTracking.Model
{
    public class Movement : IEntityBase
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }


        [JsonProperty(PropertyName = "date")]
        public DateTime? Date { get; set; }
    }
}
