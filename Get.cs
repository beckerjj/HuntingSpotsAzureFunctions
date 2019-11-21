using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Security.Claims;

namespace HuntingSpots
{
    public static class Get
    {
        [FunctionName("Get")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Get")] HttpRequest req,
            [Table("Spots")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //var test1 = ClaimsPrincipal.Current.IsInRole("Use Access of Hunting Spots");
            //var test2 = ClaimsPrincipal.Current.IsInRole("941de327-18f5-48a8-a20b-15f05641fe2f");
            //var identity = ClaimsPrincipal.Current.Identity;
            //var identities = ClaimsPrincipal.Current.Identities;
            //var claims = ClaimsPrincipal.Current.Claims;

            var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(new TableQuery<Spot>(), null);
            var spots = tableQuerySegment.Results;

            return new JsonResult(spots);
        }
    }
}
