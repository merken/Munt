using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Munt.Contract;
using Netto.AdvancesComponent.Domain;
using Prise.Plugin;

namespace Netto.AdvancesComponent
{
    [PluginBootstrapper(PluginType = typeof(AdvancesComponent))]
    public class AdvancesComponentBootstrapper : IPluginBootstrapper
    {
        [BootstrapperService(ServiceType = typeof(IConfigurationService), ProxyType = typeof(ConfigurationServiceProxy))]
        private readonly IConfigurationService configurationService;
        // TODO safety check did you mean BootstrapperService?
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            return
                // Configure the HttpClient required for the AdvancesService
                services
                .AddScoped<HttpClient>(s =>
                    {
                        var handler = new HttpClientHandler();
                        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        var client = new HttpClient(handler);
                        var baseUrl = configurationService.GetConfigurationValueForKey("Advances:BaseUrl");
                        client.BaseAddress = new Uri(baseUrl);
                        return client;
                    })
                // Configure the AdvancesService
                .AddScoped<IAdvancesService, AdvancesService>()
            ;
        }
    }
}