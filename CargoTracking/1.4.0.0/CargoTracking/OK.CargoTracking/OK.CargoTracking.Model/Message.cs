using Newtonsoft.Json;
using System.Collections.Generic;

namespace OK.CargoTracking.Model
{
    public class Message : IEntityBase
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "buttons")]
        public List<MessageButton> Buttons { get; set; }

        public Message()
        {
            this.Buttons = new List<MessageButton>();
        }
    }
}
