using System;

namespace Katalyse.Functions.Exceptions
{
    public class AppSettingsMissingException : Exception
    {
        public AppSettingsMissingException(string appSettingsName) : base($"The app settings {appSettingsName} is missing from app configuration.")
        { }
    }
}