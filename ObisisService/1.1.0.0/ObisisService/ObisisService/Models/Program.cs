using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    enum ProgramType{ Major, Minor };
    [DataContract]
    class Program
    {
        [DataMember]
        public string Faculty { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public int Class { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public List<Period> Periods { get; set; }
        [DataMember]
        public ProgramType Type { get; set; }
    }
}
