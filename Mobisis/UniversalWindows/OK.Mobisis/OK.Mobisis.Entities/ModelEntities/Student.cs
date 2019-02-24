using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ModelEntities
{
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "programs")]
        public List<Program> Programs { get; set; }

        [JsonProperty(PropertyName = "graduation")]
        public List<Graduation> Graduation { get; set; }

        [JsonProperty(PropertyName = "documents")]
        public List<Document> Documents { get; set; }
    }
}