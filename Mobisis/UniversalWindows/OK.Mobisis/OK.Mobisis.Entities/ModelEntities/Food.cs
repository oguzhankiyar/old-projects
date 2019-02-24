using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Food
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}