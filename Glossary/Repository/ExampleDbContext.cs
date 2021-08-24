using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<EmployeeEntity>().HasKey(x => x.Id);
        }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
    }
}
