using System.Collections.Generic;
using System.Threading.Tasks;

namespace Katalyse.Functions.Services
{
    public interface IContainerService
    {
        Task<string> CreateContainerAsync(string imageName, IDictionary<string, string> envVars);
        Task DeleteContainerAsync(string resourceId);
    }
}