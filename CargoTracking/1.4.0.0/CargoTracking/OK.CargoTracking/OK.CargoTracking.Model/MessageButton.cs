using Newtonsoft.Json;

namespace OK.CargoTracking.Model
{
    public class MessageButton : IEntityBase
    {
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        public MessageButton(string content, string action)
        {
            this.Content = content;
            this.Action = action;
        }
    }
}
