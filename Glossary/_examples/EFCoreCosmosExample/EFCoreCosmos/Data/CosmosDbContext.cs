using EFCoreCosmos.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCosmos.Data;

internal class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(profile =>
        {
            profile.HasKey(x => x.ProfileId);
            profile.HasPartitionKey(x => x.PartitionKey);

            profile.HasNoDiscriminator();

            profile.UseETagConcurrency();
        });
    }
}
