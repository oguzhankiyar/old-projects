﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class Activity : ObjectModel
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }
}
