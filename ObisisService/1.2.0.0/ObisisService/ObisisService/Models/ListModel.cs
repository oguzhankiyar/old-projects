using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class ListModel : ObjectModel
    {
        [DataMember]
        public ObjectModel[] List { get; set; }
    }
}