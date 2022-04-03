using CosmosSdkExample.Entities;
using CosmosSdkExample.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;

namespace CosmosSdkExample.HostedServices;

internal class ProfileService : BackgroundService
{
    private readonly CosmosClient _cosmosClient;

    public ProfileService(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _cosmosClient.CreateDatabaseIfNotExistsAsync("Sdk");

        var database = _cosmosClient.GetDatabase("Sdk");

        await database.CreateContainerIfNotExistsAsync("Profiles", "/PartitionKey");

        var container = database.GetContainer("Profiles");

        var profiles = await container.GetItemQueryIterator<Profile>().AsAsyncEnumerable().ToListAsync();

        await container.CreateItemAsync(new Profile
        {
            EmailAddress = "f@f.com",
            PartitionKey = "abc",
            ProfileId = Guid.NewGuid().ToString(),
            Things =
            {
                new StringPreference { Key = "a", Setting = "1" },
                new IntegerPreference { Key = "b", Setting = 2 },
                new StringPreference { Key = "c", Setting = "3" }
            }
        });
    }
}
