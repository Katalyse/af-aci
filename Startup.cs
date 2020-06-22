using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Katalyse.Functions.Services;

[assembly: FunctionsStartup(typeof(Katalyse.Functions.Startup))]

namespace Katalyse.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IAppSettings, EnvironmentVariableAppSettings>();
            builder.Services.AddSingleton<FunctionConfig>();
            builder.Services.AddSingleton<AzureContainerInstanceServiceConfig>();
            builder.Services.AddSingleton<IContainerService, AzureContainerInstanceService>();

        }
    }
}