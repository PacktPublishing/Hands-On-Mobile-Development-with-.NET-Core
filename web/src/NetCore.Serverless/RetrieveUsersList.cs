using System;
using System.Collections;
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
    public static class RetrieveUsersList
    {
[FunctionName("RetrieveUsersList")]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    ILogger log)
{
    // TODO: Retrieve users from UsersCollection
    var users = new List<User>();
    users.Add(new User{ Id = Guid.NewGuid().ToString(), Email = "can.bilgin@authoritypartners.com", FirstName = "Can"});

    return (ActionResult)new OkObjectResult(users);
}

        public class User
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string Email { get; set; }

            public List<string> InterestedBrands { get; set; }
        }
    }
}
