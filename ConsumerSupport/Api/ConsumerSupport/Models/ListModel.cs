﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ConsumerSupport.Models
{
    [DataContract]
    public class ListModel
    {
        [DataMember]
        public ObjectModel[] List { get; set; }
    }
}