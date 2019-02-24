using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ApiEntities
{
    public class StudentObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
