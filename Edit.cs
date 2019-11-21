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
    public static class Edit
    {
        [FunctionName("Edit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "edit")] HttpRequest req,
            [Table("Spots")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var spot = JsonConvert.DeserializeObject<Spot>(requestBody);

                if (String.IsNullOrWhiteSpace(spot?.RowKey))
                {
                    return new BadRequestResult();
                }

                //var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(new TableQuery<Spot>().Where(TableQuery.GenerateFilterCondition(nameof(Spot.RowKey), QueryComparisons.Equal, spot.RowKey)), null);
                //var existingSpot = tableQuerySegment.Results.FirstOrDefault();
                var existingSpotTableResult = await table.ExecuteAsync(TableOperation.Retrieve<Spot>(Spot.PARTITION_KEY, spot.RowKey));
                var existingSpot = existingSpotTableResult.Result as Spot;

                if (existingSpot == null)
                {
                    return new NotFoundObjectResult($"No spot found with row key {spot.RowKey}");
                }

                existingSpot.Name = spot.Name;
                existingSpot.Latitude = spot.Latitude;
                existingSpot.Longitude = spot.Longitude;

                await table.ExecuteAsync(TableOperation.Replace(existingSpot));

                //return (ActionResult)new CreatedResult($"Created spot with id spot.Id");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                log.LogError($"An error occured editing a spot!", ex);
                //This right, do we just throw?
                return new BadRequestObjectResult($"An error occured editing a spot, {ex.Message}");
            }
        }
    }
}
