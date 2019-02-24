using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OK.CargoTracking.Model
{
    public class Tracking : IEntityBase
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "factory")]
        public Factory Factory { get; set; }

        [JsonProperty(PropertyName = "lastState")]
        public string LastState { get; set; }

        [JsonProperty(PropertyName = "movements")]
        public List<Movement> Movements { get; set; }

        [JsonProperty(PropertyName = "shippedUnit")]
        public string ShippedUnit { get; set; }

        [JsonProperty(PropertyName = "arrivalUnit")]
        public string ArrivalUnit { get; set; }

        [JsonProperty(PropertyName = "deliveredBy")]
        public string DeliveredBy { get; set; }

        [JsonProperty(PropertyName = "isDelivered")]
        public bool IsDelivered { get; set; }

        [JsonProperty(PropertyName = "shippedAt")]
        public DateTime? ShippedAt { get; set; }

        [JsonProperty(PropertyName = "deliveredAt")]
        public DateTime? DeliveredAt { get; set; }

        public Tracking()
        {
            this.Movements = new List<Movement>();
        }
    }
}
