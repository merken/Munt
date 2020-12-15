using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Storage.Blob;
using Munt.Contract;
using Munt.Functions.Models;

namespace Munt.Functions
{
    public static class JourneyFunction
    {
        [FunctionName("JourneyFunction")]
        public static async Task Run(
            [QueueTrigger("munt-journey-queue", Connection = "AzureWebJobsStorage")]
            string message,
            [Blob("journeys", FileAccess.Read, Connection = "AzureWebJobsStorage")]
            CloudBlobContainer journeysContainer,
            [Queue("munt-component-queue"), StorageAccount("AzureWebJobsStorage")]
            ICollector<string> componentQueue,
            [Queue("munt-email-queue"), StorageAccount("AzureWebJobsStorage")]
            ICollector<string> emailQueue,
            ILogger log)
        {
            log.LogInformation($@"Journey triggered with message {message}");

            var journeyMessage = DeserializeJourneyMessage(message);
            var journey = journeyMessage.Journey;
            var amountForCalculationArea = journeyMessage.AmountForCalculationArea;
            var intermediateResult = amountForCalculationArea;
            var calculationResults = journeyMessage.CalculationResults ?? new System.Collections.Generic.List<CalculationResult>();

            foreach (var area in journeyMessage.Journey.Areas.OrderBy(a => a.Order))
            {
                // This component has been processed, increase the intermediate result for the calculation area here
                intermediateResult +=
                    calculationResults
                        .Where(c => c.CalculationArea == area.Order)
                        .Sum(c => c.Value);

                foreach (var component in area.Components.OrderBy(c => c.Order))
                {
                    if (IsProcessed(component, journey.BreadCrumbs))
                        continue;

                    var componentContext = new ComponentContext
                    {
                        CalculationAreaOrder = area.Order,
                        Order = component.Order,
                        AmountForCalculationArea = amountForCalculationArea,
                        IntermediateResult = intermediateResult,
                        CalculationResults = calculationResults
                    };

                    componentQueue.Add(JsonConvert.SerializeObject(new ComponentMessage
                    {
                        MuntContext = journeyMessage.Context,
                        ComponentContext = componentContext,
                        ComponentType = component.Type,
                        ComponentVersion = component.Version,
                        Journey = journey
                    }));
                    return; // stop function execution, wait for the component to re-queue a journey message
                }

                // This area has been processed, increase the amount for the calculation area here
                amountForCalculationArea =
                    calculationResults.Where(c => c.CalculationArea == area.Order)
                        .Sum(c => c.Value);
                // reset the intermediateResult for the next area
                intermediateResult = amountForCalculationArea;
            }

            // At the end, send an email with the results
            emailQueue.Add(JsonConvert.SerializeObject(new EmailMessage
            {
                Employee = $"{journeyMessage.Context.EmployeeInformation.FirstName} {journeyMessage.Context.EmployeeInformation.LastName}",
                StartDate = journeyMessage.Context.CalculationInformation.StartDate.Date,
                EndDate = journeyMessage.Context.CalculationInformation.EndDate.Date,
                EmailAddress = journeyMessage.Context.EmployeeInformation.Email,
                CalculationResults = calculationResults.ToArray(),
            }));
        }

        private static JourneyMessage DeserializeJourneyMessage(string journeyMessage)
        {
            return JsonConvert.DeserializeObject<JourneyMessage>(journeyMessage);
        }

        private static bool IsProcessed(CalculationComponent component, string[] journeyBreadcrumbs)
        {
            if (journeyBreadcrumbs == null)
                return false;

            return Array.IndexOf(journeyBreadcrumbs, JourneyUtils.ToBreadCrumb(component)) >= 0;
        }
    }
}