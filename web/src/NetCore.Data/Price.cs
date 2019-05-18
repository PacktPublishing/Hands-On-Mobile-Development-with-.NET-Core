using Newtonsoft.Json;

namespace NetCore.Data
{
    public class Price
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }
    }
}