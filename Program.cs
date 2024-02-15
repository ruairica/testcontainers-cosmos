using Microsoft.Azure.Cosmos;
using Testcontainers.CosmosDb;

var cosmosDbContainer = new CosmosDbBuilder()
    .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1")
    .Build();

await cosmosDbContainer.StartAsync().ConfigureAwait(false);

CosmosClient client = new(
            cosmosDbContainer.GetConnectionString(),
            new CosmosClientOptions
            {
                HttpClientFactory = () => cosmosDbContainer.HttpClient, // Using the container's HttpClient which should be trusted.
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });

await client.CreateDatabaseIfNotExistsAsync(
    id: "DATABASE_NAME",
    throughput: 400);

Console.WriteLine("End");