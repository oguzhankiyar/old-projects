using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    class Graduation
    {
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String Condition { set; get; }
    }
}
