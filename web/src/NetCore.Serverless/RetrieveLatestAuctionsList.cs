using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NetCore.Serverless
{
    public static class RetrieveLatestAuctionsList
    {
[FunctionName("RetrieveLatestAuctionsList")]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
{
    // TODO: Retrieve latest auctions from AuctionsCollection
    var auctions = new List<Auction>();
    auctions.Add(new Auction
    {
        Brand = "Volvo",
        Model = "S60",
        Year = 2017,
        CurrentHighestBid = 26000,
        StartingPrice = 25000
    });

    return (ActionResult)new OkObjectResult(auctions);
}
    }

    public class Auction
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int StartingPrice { get; set; }

        public int CurrentHighestBid { get; set; }
    }
}
