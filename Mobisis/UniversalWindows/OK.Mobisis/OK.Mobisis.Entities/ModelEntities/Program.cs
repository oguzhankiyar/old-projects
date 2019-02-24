using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.Enums;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Program
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public ProgramType Type { get; set; }

        [JsonProperty(PropertyName = "faculty")]
        public string Faculty { get; set; }

        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }

        [JsonProperty(PropertyName = "class")]
        public int Class { get; set; }

        [JsonProperty(PropertyName = "gano")]
        public double GANO { get; set; }

        [JsonProperty(PropertyName = "periods")]
        public List<Period> Periods { get; set; }
    }
}
