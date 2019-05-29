using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.OData;
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
}
