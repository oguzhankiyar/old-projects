using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    class Period
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public int YearCode { get; set; }
        [DataMember]
        public string Year { set; get; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public List<Lesson> Lessons { get; set; }
    }
}
