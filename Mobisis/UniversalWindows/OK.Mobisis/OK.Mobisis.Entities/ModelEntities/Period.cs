using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Period
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "yearCode")]
        public int YearCode { get; set; }

        [JsonProperty(PropertyName = "programCode")]
        public int ProgramCode { get; set; }

        [JsonProperty(PropertyName = "year")]
        public string Year { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gano")]
        public double GANO { get; set; }

        [JsonProperty(PropertyName = "lessons")]
        public List<Lesson> Lessons { get; set; }

    }
}
