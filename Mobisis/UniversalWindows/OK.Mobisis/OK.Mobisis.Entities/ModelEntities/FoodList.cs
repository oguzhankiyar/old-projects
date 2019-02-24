using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class FoodList
    {
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "foods")]
        public List<Food> Foods { get; set; }
    }
}