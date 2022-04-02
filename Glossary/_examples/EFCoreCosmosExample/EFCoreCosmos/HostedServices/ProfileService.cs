using EFCoreCosmos.Data;
using EFCoreCosmos.Entities;
using Microsoft.Extensions.Hosting;

namespace EFCoreCosmos.HostedServices;

internal class ProfileService : BackgroundService
{
    private readonly CosmosDbContext _dbContext;

    public ProfileService(
        CosmosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();

        _dbContext.Profiles.Add(new Profile
        {
            ProfileId = Guid.NewGuid().ToString(),
            PartitionKey = "abc",
            EmailAddress = "f@f.com",
            Things =
            {
                new StringPreference { Key = "pref1", Setting = "1" },
                new IntegerPreference { Key = "pref2", Setting = 2 },
                new StringPreference { Key = "pref3", Setting = "3" }
            }
        });

        await _dbContext.SaveChangesAsync();
    }
}
