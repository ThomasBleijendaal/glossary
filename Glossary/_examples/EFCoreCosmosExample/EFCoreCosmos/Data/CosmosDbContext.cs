using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            profile.OwnsMany(x => x.Things);
        });

        modelBuilder.Owned<Preference>();

        modelBuilder.Entity<Preference>(preference =>
        {
            preference.HasDiscriminator()
                .HasValue<StringPreference>("string")
                .HasValue<IntegerPreference>("integer");
        });
    }
}
