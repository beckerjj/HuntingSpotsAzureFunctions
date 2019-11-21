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
using System.Linq;

namespace HuntingSpots
{
    public static class GetOne
    {
        [FunctionName("GetOne")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetOne")] HttpRequest req,
            [Table("Spots")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var query = System.Web.HttpUtility.ParseQueryString(req.QueryString.Value);
            string rowKey = query.Get("rowKey");

            if (String.IsNullOrWhiteSpace(rowKey))
            {
                return new BadRequestResult();
            }

            var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(new TableQuery<Spot>().Where(TableQuery.GenerateFilterCondition(nameof(Spot.RowKey), QueryComparisons.Equal, rowKey)), null);
            var spot = tableQuerySegment.Results.FirstOrDefault();

            if (spot == null)
            {
                return new NotFoundObjectResult($"No spot found with row key {rowKey}");
            }

            return new JsonResult(spot);
        }
    }
}
