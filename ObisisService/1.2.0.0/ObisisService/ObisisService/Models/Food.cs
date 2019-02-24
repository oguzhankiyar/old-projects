using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class Food : ObjectModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Calorie { get; set; }
    }
}
