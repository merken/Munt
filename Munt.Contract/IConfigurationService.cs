using System;

namespace Munt.Contract
{
    public interface IConfigurationService
    {
        string GetConfigurationValueForKey(string key);
    }
}