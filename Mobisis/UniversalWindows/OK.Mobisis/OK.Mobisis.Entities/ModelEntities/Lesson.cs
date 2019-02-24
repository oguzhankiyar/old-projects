using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OK.Mobisis.Entities.Enums;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Lesson
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "credit")]
        public double Credit { get; set; }

        [JsonProperty(PropertyName = "firstMidterm")]
        public Exam FirstMidterm { get; set; }

        [JsonProperty(PropertyName = "secondMidterm")]
        public Exam SecondMidterm { get; set; }

        [JsonProperty(PropertyName = "thirdMidterm")]
        public Exam ThirdMidterm { get; set; }

        [JsonProperty(PropertyName = "final")]
        public Exam Final { get; set; }

        [JsonProperty(PropertyName = "integration")]
        public Exam Integration { get; set; }

        [JsonProperty(PropertyName = "integrationRight")]
        public bool? IntegrationRight { get; set; }

        [JsonProperty(PropertyName = "average")]
        public int? Average { get; set; }

        [JsonProperty(PropertyName = "averageEffect")]
        public bool AverageEffect { get; set; }

        [JsonProperty(PropertyName = "successFactor")]
        public double SuccessFactor { get; set; }

        [JsonProperty(PropertyName = "class")]
        public int Class { get; set; }

        [JsonProperty(PropertyName = "grade")]
        public GradeType? Grade { get; set; }

        [JsonProperty(PropertyName = "state")]
        public LessonState? State { get; set; }

        [JsonProperty(PropertyName = "absentState")]
        public AbsentType? AbsentState { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }
}
