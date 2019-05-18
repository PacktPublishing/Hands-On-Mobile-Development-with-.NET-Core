using Newtonsoft.Json;

namespace NetCore.Data
{
    public class Fuel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}