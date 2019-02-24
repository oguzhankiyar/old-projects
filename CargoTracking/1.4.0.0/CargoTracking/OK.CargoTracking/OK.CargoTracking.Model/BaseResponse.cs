using Newtonsoft.Json;
using System.Collections.Generic;

namespace OK.CargoTracking.Model
{
    public class BaseResponse
    {
        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }

        [JsonProperty(PropertyName = "result")]
        public object Result { get; set; }

        public BaseResponse()
        {
            this.Status = true;
        }
    }
}