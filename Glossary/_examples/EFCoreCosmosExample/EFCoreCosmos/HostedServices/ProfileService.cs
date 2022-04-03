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
                { "pref1", "1" },
                { "pref2", "2" },
                { "pref3", "3" }
            }
        });

        await _dbContext.SaveChangesAsync();
    }
}
