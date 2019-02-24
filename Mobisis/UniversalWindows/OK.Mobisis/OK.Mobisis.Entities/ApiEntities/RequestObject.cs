using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.Enums;

namespace OK.Mobisis.Entities.ApiEntities
{
    public class RequestObject
    {
        [JsonProperty(PropertyName = "type")]
        public RequestType Type { get; set; }

        [JsonProperty(PropertyName = "student")]
        public StudentObject Student { get; set; }

        [JsonProperty(PropertyName = "lesson")]
        public LessonObject Lesson { get; set; }

        [JsonProperty(PropertyName = "period")]
        public PeriodObject Period { get; set; }

        [JsonProperty(PropertyName = "program")]
        public ProgramObject Program { get; set; }
    }
}
