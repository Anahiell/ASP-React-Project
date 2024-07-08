using Microsoft.Azure.Cosmos;

namespace AzureSpu221MyV.Services.Data
{
    public class CosmosDbContainerProvider(IConfiguration configuration) : IContainerProvider
    {
        private readonly IConfiguration _configuration = configuration;
        private CosmosClient? client;
        private Database? database;
        private Container? container;
        public async Task<Container> GetContainerAsync()
        {

            if (container == null)
            {
                var cosmosSection = (_configuration
                ?.GetSection("azure")
                ?.GetSection("cosmosDb"))
                ?? throw new Exception("Congiguration load error(missing azzure.json)");
                if (cosmosSection == null)
                {
                    throw new Exception("Configuration load error (missing azuresetting.json'?)");
                }
                String? endpointUri = cosmosSection.GetSection("endpointUri").Value
                    ?? throw new Exception("Configuration parse error(missing 'endpointUri'");
                String? primaryKey = cosmosSection.GetSection("primaryKey").Value
                    ?? throw new Exception("Configuration parse error(missing 'primaryKey')");

                String databaseId = cosmosSection.GetSection("databaseId").Value
                    ?? throw new Exception("Configuration parse error(missing 'databaseUri'");
                String containerId = cosmosSection.GetSection("containerId").Value
                    ?? throw new Exception("Configuration parse error(missing 'containerId'");

                this.client = new(endpointUri, primaryKey,
                new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });

                this.database = await this.client.CreateDatabaseIfNotExistsAsync(databaseId);

                this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

            }
            return container!;

        }
    }
}

