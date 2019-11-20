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

namespace HuntingSpots
{
    public static class Get
    {
        [FunctionName("Get")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "api/Get")] HttpRequest req,
            [Table("Spots")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(new TableQuery<Spot>(), null);
            var spots = tableQuerySegment.Results;

            return new JsonResult(spots);
        }
    }
}
