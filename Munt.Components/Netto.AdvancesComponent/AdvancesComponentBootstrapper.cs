using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Munt.Contract;
using Prise.Plugin;

namespace Netto.AdvancesComponent
{
    [PluginBootstrapper(PluginType = typeof(MuntCalculationComponent))]
    public class AdvancesComponentBootstrapper : IPluginBootstrapper
    {
        [PluginService(ServiceType = typeof(IConfigurationService), ProvidedBy = ProvidedBy.Host, ProxyType = typeof(ConfigurationServiceProxy))]
        private readonly IConfigurationService configurationService;
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Configure the HttpClient required for the Advances Component
            return services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient();
                var baseUrl = configurationService.GetConfigurationValueForKey("Advances:BaseUrl");
                client.BaseAddress = new Uri(baseUrl);
                return client;
            });
        }
    }
}