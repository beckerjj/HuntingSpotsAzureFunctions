using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HuntingSpots
{
    public static class Create
    {
        [FunctionName("Create")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create")] HttpRequest req,
            [Table("Spots", Connection = "AzureWebJobsStorage")] IAsyncCollector<Spot> spotTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var spot = JsonConvert.DeserializeObject<Spot>(requestBody);
                spot.RowKey = Guid.NewGuid().ToString();

                log.LogInformation($"Creating:\r\n{JsonConvert.SerializeObject(spot, Formatting.Indented)}");

                await spotTable.AddAsync(spot);

                //return (ActionResult)new CreatedResult($"Created spot with id spot.Id");
                return (ActionResult)new OkObjectResult($"Created spot with row key {spot.RowKey}");
            }
            catch (Exception ex)
            {
                log.LogError($"An error occured creating a spot!", ex);
                //This right, do we just throw?
                return new BadRequestObjectResult($"An error occured creating a spot, {ex.Message}");
            }
        }
    }
}
