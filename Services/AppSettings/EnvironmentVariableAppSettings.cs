using System;
using Katalyse.Functions.Exceptions;

namespace Katalyse.Functions.Services
{
    public class EnvironmentVariableAppSettings : IAppSettings
    {
        public string GetAppSettings(string name)
        {
            return Environment.GetEnvironmentVariable(name) ?? throw new AppSettingsMissingException(name);
        }
    }
}