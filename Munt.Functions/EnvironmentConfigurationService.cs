using System;
using Munt.Contract;

namespace Munt.Functions
{
    public class EnvironmentConfigurationService : IConfigurationService
    {
        public string GetConfigurationValueForKey(string key)
        {
            return System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        }
    }
}