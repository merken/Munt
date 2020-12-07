using Munt.Contract;
using Prise.Proxy;

namespace Netto.AdvancesComponent
{
    public class ConfigurationServiceProxy : ReverseProxy, IConfigurationService
    {
        public ConfigurationServiceProxy(object hostService) : base(hostService) { }
        public string GetConfigurationValueForKey(string key) => this.InvokeOnHostService<string>(key);
    }
}