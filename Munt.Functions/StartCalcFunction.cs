using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Storage.Blob;
using Munt.Contract;
using Munt.Functions.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Munt.Functions
{
    public static class StartCalcFunction
    {
        [FunctionName("StartCalc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "StartCalc/{journey}")]
            HttpRequest req,
            [Blob("journeys", FileAccess.Read, Connection = "AzureWebJobsStorage")]
            CloudBlobContainer journeysContainer,
            [Queue("munt-journey-queue"), StorageAccount("AzureWebJobsStorage")]
            ICollector<string> queue,
            string journey,
            ILogger log)
        {
            var context = await DeserializeMuntContext(req.Body);

            var journeyBlobFileName = $"{journey}.json";

            if (!journeysContainer.GetBlockBlobReference(journeyBlobFileName).Exists())
                throw new NotSupportedException($"Journey {journeyBlobFileName} was not found.");
            var journeyFromBlob =
                await journeysContainer.GetBlockBlobReference(journeyBlobFileName).DownloadTextAsync();
            var journeyObject = DeserializeJourney(journeyFromBlob);

            var journeyMessage = new JourneyMessage
            {
                Context = context,
                Journey = journeyObject
            };

            log.LogInformation($@"Starting Munt calculation for 
                Journey: {journey} 
                Period: {context.CalculationInformation.StartDate.Date.ToShortDateString()} - {context.CalculationInformation.EndDate.Date.ToShortDateString()}
                Employee: {context.EmployeeInformation.FirstName} {context.EmployeeInformation.LastName} 
                Employer: {context.EmployerInformation.Id}");

            queue.Add(JsonSerializer.Serialize(journeyMessage));

            return (ActionResult) new OkObjectResult($"Ok");
        }

        private static async Task<MuntContext> DeserializeMuntContext(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<MuntContext>(new StreamReader(stream).BaseStream,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = {new JsonStringEnumConverter()}
                });
        }

        private static Journey DeserializeJourney(string journey)
        {
            return JsonConvert.DeserializeObject<JourneyRootObject>(journey).Journey;
        }
    }
}