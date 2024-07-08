using Microsoft.Azure.Cosmos;

namespace AzureSpu221MyV.Services.Data
{
    public interface IContainerProvider
    {
        Task<Container> GetContainerAsync();
    }
}
