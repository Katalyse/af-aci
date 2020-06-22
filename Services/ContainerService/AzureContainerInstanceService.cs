using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace Katalyse.Functions.Services
{
    public class AzureContainerInstanceService : IContainerService
    {
        private readonly AzureContainerInstanceServiceConfig config;

        public AzureContainerInstanceService(AzureContainerInstanceServiceConfig config)
        {
            this.config = config;
        }

        public async Task<string> CreateContainerAsync(string imageName, IDictionary<string, string> envVars)
        {
            var context = GetContext();
            var containerName = BuildContainerName();
            var rg = await context.ResourceGroups.GetByNameAsync(config.ResourceGroupName);

            //Feel free to setup your Azure Container Instance definition as you need.
            var group = context.ContainerGroups.Define(containerName)
                .WithRegion(rg.RegionName)
                .WithExistingResourceGroup(rg.Name)
                .WithLinux()
                .WithPrivateImageRegistry(config.ContainerRegistryServer, config.ContainerRegistryUsername, config.ContainerRegistryPassword)
                .WithoutVolume()
                .DefineContainerInstance(containerName)
                .WithImage(imageName)
                .WithoutPorts()
                .WithEnvironmentVariablesWithSecuredValue(envVars)
                .Attach()
                .WithRestartPolicy(ContainerGroupRestartPolicy.Never)
                .Create();

            return group.Id;
        }

        public async Task DeleteContainerAsync(string resourceId)
        {
            var context = GetContext();
            await context.ContainerGroups.DeleteByIdAsync(resourceId);
        }

        private IAzure GetContext()
        {
            var credentials = new AzureCredentialsFactory().FromServicePrincipal(
                                config.ClientId,
                                config.ClientSecret,
                                config.TenantId,
                                AzureEnvironment.AzureGlobalCloud);

            return Azure.Authenticate(credentials).WithDefaultSubscription();
        }

        private string BuildContainerName()
        {
            return $"{config.ContainerNamePrefix}-{Guid.NewGuid().ToString("N")}";
        }
    }
}