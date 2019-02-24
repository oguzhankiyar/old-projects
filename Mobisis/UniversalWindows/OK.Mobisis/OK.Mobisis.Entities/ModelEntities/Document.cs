using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.Enums;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Document
    {
        [JsonProperty(PropertyName = "program")]
        public ProgramType Program { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "requestCount")]
        public int RequestCount { get; set; }

        [JsonProperty(PropertyName = "printDate")]
        public DateTime PrintDate { get; set; }

        [JsonProperty(PropertyName = "printCount")]
        public int PrintCount { get; set; }

        [JsonProperty(PropertyName = "isPrinted")]
        public bool IsPrinted { get; set; }

        [JsonProperty(PropertyName = "downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}
