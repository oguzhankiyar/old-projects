using Newtonsoft.Json;

namespace OK.CargoTracking.Model
{
    public class Factory : IEntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "iconSource")]
        public string IconSource { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public int Sort { get; set; }

        public Factory() { }

        public Factory(int id, string name, string source)
        {
            this.Id = id;
            this.Name = name;
            this.IconSource = source;
        }
    }
}
