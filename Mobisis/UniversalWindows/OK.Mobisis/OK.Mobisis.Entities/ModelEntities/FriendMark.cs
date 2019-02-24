using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class FriendMark
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "firstMidterm")]
        public int? FirstMidterm { get; set; }

        [JsonProperty(PropertyName = "secondMidterm")]
        public int? SecondMidterm { get; set; }

        [JsonProperty(PropertyName = "thirdMidterm")]
        public int? ThirdMidterm { get; set; }

        [JsonProperty(PropertyName = "final")]
        public int? Final { get; set; }

        [JsonProperty(PropertyName = "integration")]
        public int? Integration { get; set; }

        [JsonProperty(PropertyName = "gano")]
        public double GANO { get; set; }
    }
}
