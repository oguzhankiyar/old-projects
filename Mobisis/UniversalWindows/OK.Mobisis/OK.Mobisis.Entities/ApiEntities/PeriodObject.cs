using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ApiEntities
{
    public class PeriodObject
    {
        [JsonProperty(PropertyName = "studentId")]
        public string StudentId { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "yearCode")]
        public int YearCode { get; set; }

        [JsonProperty(PropertyName = "programCode")]
        public int ProgramCode { get; set; }
    }
}
