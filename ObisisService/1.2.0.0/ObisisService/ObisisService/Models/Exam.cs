using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class Exam : ObjectModel
    {
        [DataMember]
        public int? Mark { get; set; }
        [DataMember]
        public DateTime? Date { get; set; }
    }
}
