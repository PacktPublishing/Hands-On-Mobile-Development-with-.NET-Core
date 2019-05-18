using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NetCore.Serverless
{
    public static class AuctionsTrigger
    {
        [FunctionName("AuctionsTrigger")]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "ProductsDb",
                collectionName: "AuctionsCollection",
                ConnectionStringSetting = "DbConnection",
                LeaseCollectionPrefix = "AuctionsTrigger",
                LeaseCollectionName = "LeasesCollection")]IReadOnlyList<Document> input,
            [CosmosDB(
                databaseName: "ProductsDb",
                collectionName: "UsersCollection",
                ConnectionStringSetting = "DbConnection")] DocumentClient client,
            ExecutionContext context,
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
