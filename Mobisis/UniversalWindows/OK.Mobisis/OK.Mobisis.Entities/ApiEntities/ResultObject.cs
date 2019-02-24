using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace OK.Mobisis.Entities.ApiEntities
{
    public class ResultObject<T>
    {
        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "response")]
        public T Response { get; set; }

        public ResultObject()
        {
            this.Status = true;
        }
    }
}
