using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HuntingSpots
{
    public static class GetOne
    {
        [FunctionName("GetOne")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetOne/{rowKey?}")] HttpRequest req,
            [Table("Spots", Spot.PARTITION_KEY, "{rowKey}")] Spot spot,
            string rowKey,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return spot != null
                ? (ActionResult) new JsonResult(spot)
                : new NotFoundObjectResult($"No spot found with row key {rowKey}");
        }
    }
}
