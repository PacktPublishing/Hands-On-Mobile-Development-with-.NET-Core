using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NetCore.Data;
using NetCore.Data.Cosmos;

namespace NetCore.Web.UsersApi.Controllers
{
    public class UsersController : ODataController
    {
        private readonly IDistributedCache _distributedCache;

        public UsersController(IDistributedCache distributedCacheInstance)
        {
            _distributedCache = distributedCacheInstance;
        }

        // GET: api/Users
        [EnableQuery]
        public IActionResult Get()
        {
            var cosmosCollection = new CosmosCollection<User>("UsersCollection");
            var items = cosmosCollection.GetItemsAsync();

            return Ok(items.AsQueryable());
        }

        [EnableQuery]
        public IActionResult Get(string key)
        {
            var cosmosCollection = new CosmosCollection<User>("UsersCollection");
            return Ok(cosmosCollection.GetItemAsync(key));
        }
    }

    public class AuctionsController : ODataController
    {
        // GET: api/Users
        [EnableQuery]
        public IActionResult Get()
        {
            var cosmosCollection = new CosmosCollection<Auction>("AuctionsCollection");
            var items = cosmosCollection.GetItemsAsync();

            return Ok(items.AsQueryable());
        }

        [EnableQuery]
        public async Task<IActionResult> Get(string key)
        {
            // Get the version stamp of the entity
            string entityTag = string.Empty;

            if (Request.Headers.ContainsKey("If-None-Match"))
            {
                entityTag = Request.Headers["If-None-Match"].First();
            }

            var cosmosCollection = new CosmosCollection<Auction>("AuctionsCollection");
            var resultantSet = await cosmosCollection.GetItemsAsync(item => item.Id == key);
            var auction = resultantSet.FirstOrDefault();

            if (auction == null)
            {
                return NotFound();
            }

            if (int.TryParse(entityTag, out int timeStamp) && auction.TimeStamp == timeStamp)
            {
                return StatusCode((int)HttpStatusCode.NotModified);

                
            }

            return Ok(auction);
        }

        [EnableQuery]
        [HttpPatch]
        public async Task<IActionResult> Patch(string key, [FromBody] JsonPatchDocument<Auction> auctionPatch)
        {
            var cosmosCollection = new CosmosCollection<Auction>("AuctionsCollection");
            var auction = (await cosmosCollection.GetItemsAsync(item => item.Id == key)).FirstOrDefault();

            if (auction == null)
            {
                return NotFound();
            }

            auctionPatch.ApplyTo(auction);

            await cosmosCollection.UpdateItemAsync(key, auction);

            return Accepted(auction);
        }

        [EnableQuery]
        [HttpPut]
        public async Task<IActionResult> Put([FromODataUri]string key, [FromBody] Auction auctionUpdate)
        {
            var cosmosCollection = new CosmosCollection<Auction>("AuctionsCollection");
            var auction = (await cosmosCollection.GetItemsAsync(item => item.Id == key)).FirstOrDefault();

            if (auction == null)
            {
                return NotFound();
            }

            if (auction.TimeStamp != auctionUpdate.TimeStamp)
            {
                return Conflict();
            }

            await cosmosCollection.UpdateItemAsync(key, auctionUpdate);

            return Accepted(auction);
        }
    }
}
