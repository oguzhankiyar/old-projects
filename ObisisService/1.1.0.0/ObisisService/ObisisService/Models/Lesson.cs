using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    enum AbsentType { Free, Passed, Failed };
    [DataContract]
    class Lesson
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public double Credit { get; set; }
        [DataMember]
        public Exam FirstMidterm { get; set; }
        [DataMember]
        public Exam SecondMidterm { get; set; }
        [DataMember]
        public Exam ThirdMidterm { get; set; }
        [DataMember]
        public Exam Final { get; set; }
        [DataMember]
        public Exam Integration { get; set; }
        [DataMember]
        public bool IntegrationRight { get; set; }
        [DataMember]
        public double Average { get; set; }
        [DataMember]
        public string Grade { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public AbsentType AbsentState { get; set; }
    }
}
