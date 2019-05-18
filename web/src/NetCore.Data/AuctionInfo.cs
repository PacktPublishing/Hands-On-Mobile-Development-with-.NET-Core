using Newtonsoft.Json;

namespace NetCore.Data
{
    public class AuctionInfo
    {
        [JsonProperty("auctionId")]
        public string AuctionId { get; set; }

        [JsonProperty("auctionReview")]
        public int AuctionReview { get; set; }
    }
}