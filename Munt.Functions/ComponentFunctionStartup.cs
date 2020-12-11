using System;
using System.IO;
using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Munt.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prise.DependencyInjection;
using BaGet.Protocol;
using Prise.AssemblyScanning;
using Munt.Functions.Services;

[assembly: FunctionsStartup(typeof(Munt.Functions.ComponentFunctionStartup))]
namespace Munt.Functions
{
    public class ComponentFunctionStartup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add a Host Service 
            builder.Services.AddScoped<IConfigurationService, EnvironmentConfigurationService>();

            // Add the BaGet services
            builder.Services.AddScoped<NuGetClient, NuGetClient>((sp) =>
            {
                var configuration = sp.GetRequiredService<IConfigurationService>();
                var nugetEndpoint = configuration.GetConfigurationValueForKey("NuGetEndpoint");
                return new NuGetClient(nugetEndpoint);
            });
            builder.Services.AddScoped<INugetServerService, BaGetService>();

            // Add Prise services 
            builder.Services.AddPrise();
            // Use the Prise DefaultNugetAssemblyScanner
            builder.Services.AddFactory<IAssemblyScanner>(DefaultFactories.DefaultNuGetAssemblyScanner, ServiceLifetime.Scoped);
        }
    }
}