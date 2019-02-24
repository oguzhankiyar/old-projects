using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Exam
    {
        [JsonProperty(PropertyName = "mark")]
        public int? Mark { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime? Date { get; set; }
    }
}
