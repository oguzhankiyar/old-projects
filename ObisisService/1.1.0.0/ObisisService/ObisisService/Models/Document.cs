using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    class Document
    {
        [DataMember]
        public Program Program { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public DateTime RequestDate { get; set; }
        [DataMember]
        public int RequestCount { get; set; }
        [DataMember]
        public DateTime PrintDate { get; set; }
        [DataMember]
        public int PrintCount { get; set; }
    }
}
