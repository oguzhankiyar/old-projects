using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class FoodList : ObjectModel
    {
        [DataMember]
        public List<Food> Foods { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }
}
