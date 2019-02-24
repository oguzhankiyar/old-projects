using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    class FoodList
    {
        [DataMember]
        public List<Food> Foods { get; set; }
        [DataMember]
        public DateTime FoodDate { get; set; }
    }
}
