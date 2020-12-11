using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Munt.Contract;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Prise;
using Munt.Functions.Models;
using Munt.Contract;
using Munt.Functions;

namespace Munt.Functions
{
    public class ComponentFunction
    {
        private readonly IPluginLoader pluginLoader;
        private readonly INugetServerService nugetServerService;
        private readonly IConfigurationService configurationService;

        public ComponentFunction(
            IPluginLoader pluginLoader,
            INugetServerService nugetServerService,
            IConfigurationService configurationService)
        {
            this.pluginLoader = pluginLoader;
            this.nugetServerService = nugetServerService;
            this.configurationService = configurationService;
        }

        private static ComponentMessage Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<ComponentMessage>(message);
        }

        [FunctionName("ComponentFunction")]
        public async Task Run(
            [QueueTrigger("munt-component-queue", Connection = "AzureWebJobsStorage")]
            string message,
            [Queue("munt-journey-queue"), StorageAccount("AzureWebJobsStorage")]
            ICollector<string> journeyQueue,
            ILogger log)
        {
            var componentMessage = Deserialize(message);
            var componentType = componentMessage.ComponentType;
            var componentVersion = componentMessage.ComponentVersion;

            log.LogInformation(
                $"Component {componentType} received context {componentMessage.MuntContext} and {componentMessage.ComponentContext}");

            var nugetComponentVersions = await this.nugetServerService.GetPackageVersions(componentType);
            if (!nugetComponentVersions.Any())
                throw new ArgumentException($"Component {componentType} does not have any versions on NuGet");

            var nugetComponent =
                nugetComponentVersions.FirstOrDefault(c => c.Version.Equals(new Version(componentVersion)));
            if (nugetComponent == null)
                throw new ArgumentException(
                    $"Component {componentType} with version {componentVersion} does not exist on NuGet");

            var packageName = $"{componentType}.{componentVersion}.nupkg";
            var packageLocation =
                Path.GetFullPath(packageName); // Results to bin/Debug/netcoreapp3.1/package.version.nupkg

            if (IsRunningOnAzure())
                packageLocation =
                    Path.Combine(Path.GetTempPath(),
                        packageName); // Results to the TEMP directory of the Azure Functions (App Service) on Azure

            // Download from NuGet server if file with specific version does not exist
            if (!File.Exists(packageLocation))
            {
                var package = await this.nugetServerService.DownloadPackage(componentType, componentVersion);
                await File.WriteAllBytesAsync(packageLocation, package);
            }

            var pathToPackage =
                Path.GetDirectoryName(
                    packageLocation); // Returns the root execution directory, where all nupkg's are downloaded
            // Scanning for nupkg's also extracts all available packages
            var pluginScanResults = await this.pluginLoader.FindPlugins<IMuntCalculationComponent>(pathToPackage);
            // We're looking for the nupkg with the correct name and version
            var pluginScanResult = pluginScanResults.FirstOrDefault(p =>
                p.AssemblyPath.Contains(componentType) && p.AssemblyPath.Contains(componentVersion));
            if (pluginScanResult == null)
                throw new NotSupportedException(
                    $"Could not find extracted plugin with name {componentType} and version {componentVersion}");

            var plugin = await this.pluginLoader.LoadPlugin<IMuntCalculationComponent>(pluginScanResult,
                configure: (ctx) =>
                {
                    // Share the IConfigurationService
                    ctx.AddHostService<IConfigurationService>(this.configurationService);
                });

            var componentBreadCrumb = JourneyUtils.ToBreadCrumb(componentType, componentVersion);
            var results = await plugin.Calculate(componentMessage.MuntContext, componentMessage.ComponentContext);
            var journey = componentMessage.Journey;
            var journeyMessage = new JourneyMessage
            {
                AmountForCalculationArea = componentMessage.ComponentContext.AmountForCalculationArea,
                Context = componentMessage.MuntContext,
                Journey = AddBreadCrumb(journey, componentBreadCrumb),
                CalculationResults = AddResults(componentMessage.ComponentContext.CalculationResults, results)
            };

            var newMessage = JsonConvert.SerializeObject(journeyMessage);

            log.LogInformation($"Component {componentBreadCrumb} created new context {newMessage}");

            journeyQueue.Add(newMessage);
        }

        private List<CalculationResult> AddResults(List<CalculationResult> initialResults,
            List<CalculationResult> resultsFromComponent)
        {
            var list = new List<CalculationResult>(initialResults ?? new List<CalculationResult>());
            list.AddRange(resultsFromComponent);
            return list;
        }

        private bool IsRunningOnAzure()
        {
            return this.configurationService.GetConfigurationValueForKey("AZURE") != null &&
                   bool.Parse(this.configurationService.GetConfigurationValueForKey("AZURE"));
        }

        private Journey AddBreadCrumb(Journey journey, string breadCrumb)
        {
            var breadCrumbs = new List<string>(journey.BreadCrumbs ?? new string[] { });
            breadCrumbs.Add(breadCrumb);
            journey.BreadCrumbs = breadCrumbs.ToArray();
            return journey;
        }
    }
}