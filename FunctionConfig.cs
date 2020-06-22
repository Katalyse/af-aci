using Katalyse.Functions.Services;

namespace Katalyse.Functions
{
    public class FunctionConfig
    {
        private readonly IAppSettings settings;

        public FunctionConfig(IAppSettings settings)
        {
            this.settings = settings;
        }

        public string ImageName { get => settings.GetAppSettings("ImageName"); }
    }
}