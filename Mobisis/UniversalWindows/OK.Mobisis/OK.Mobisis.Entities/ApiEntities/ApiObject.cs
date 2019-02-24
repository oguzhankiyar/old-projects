using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ApiEntities
{
    public class ApiObject
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "request")]
        public RequestObject Request { get; set; }
    }
}