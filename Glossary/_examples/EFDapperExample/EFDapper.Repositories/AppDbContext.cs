using EFDapper.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFDapper.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; } = default!;

        public DbSet<EmailAddress> EmailAddresses { get; set; } = default!;

        public DbSet<Address> Addresses { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    Name = "Test Person 1"
                });

            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    City = "City 1",
                    Country = "Country 1",
                    Street = "Street 123",
                    Zipcode = "ZIP123",
                    PersonId = 1
                },
                new Address
                {
                    Id = 2,
                    City = "City 2",
                    Country = "Country 1",
                    Street = "Street 1234",
                    Zipcode = "ZIP1234",
                    PersonId = 1
                });

            modelBuilder.Entity<EmailAddress>().HasData(
                new EmailAddress
                {
                    Id = 1,
                    Emailaddress = "emailaddress1@example.com",
                    PersonId = 1
                },
                new EmailAddress
                {
                    Id = 2,
                    Emailaddress = "emailaddress2@example.com",
                    PersonId = 1
                },
                new EmailAddress
                {
                    Id = 3,
                    Emailaddress = "emailaddress3@example.com",
                    PersonId = 1
                });
        }
    }
}
