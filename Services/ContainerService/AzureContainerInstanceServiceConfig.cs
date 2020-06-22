namespace Katalyse.Functions.Services
{
    public class AzureContainerInstanceServiceConfig
    {
        private readonly IAppSettings settings;

        public AzureContainerInstanceServiceConfig(IAppSettings settings)
        {
            this.settings = settings;
        }

        public string ClientId { get => settings.GetAppSettings("ServicePrincipalClientId"); }
        public string ClientSecret { get => settings.GetAppSettings("ServicePrincipalClientSecret"); }
        public string TenantId { get => settings.GetAppSettings("ServicePrincipalTenantId"); }
        public string ContainerNamePrefix { get => settings.GetAppSettings("ContainerNamePrefix"); }
        public string ResourceGroupName { get => settings.GetAppSettings("ResourceGroupName"); }
        public string ContainerRegistryServer { get => settings.GetAppSettings("ContainerRegistryServer"); }
        public string ContainerRegistryUsername { get => settings.GetAppSettings("ContainerRegistryUsername"); }
        public string ContainerRegistryPassword { get => settings.GetAppSettings("ContainerRegistryPassword"); }
    }
}