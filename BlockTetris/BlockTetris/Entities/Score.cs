using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockTetris.Entities
{
    public class Score
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "deviceId")]
        public object DeviceId { get; set; }
        [JsonProperty(PropertyName = "playerName")]
        public string PlayerName { get; set; }
        [JsonProperty(PropertyName = "playerScore")]
        public int PlayerScore { get; set; }
        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }
        [JsonProperty(PropertyName = "__createdAt")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "__updatedAt")]
        public DateTime UpdatedDate { get; set; }
    }
}
