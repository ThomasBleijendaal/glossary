using EFDapper.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQueries
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; } = default!;

        public DbSet<Employee> Employees { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var emp1 = new Employee
            {
                Id = 1,
                Name = "Emp 1"
            };
            var emp2 = new Employee
            {
                Id = 2,
                Name = "Emp 2"
            };
            var emp3 = new Employee
            {
                Id = 3,
                Name = "Emp 3"
            };
            var emp4 = new Employee
            {
                Id = 4,
                Name = "Emp 4"
            };
            var emp5 = new Employee
            {
                Id = 5,
                Name = "Emp 5"
            };

            var comp1 = new Company
            {
                Id = 1,
                Name = "Company 1"
            };
            var comp2 = new Company
            {
                Id = 2,
                Name = "Company 2"
            };
            var comp3 = new Company
            {
                Id = 3,
                Name = "Company 3"
            };

            emp1.CompanyId = comp1.Id;
            emp2.CompanyId = comp1.Id;
            emp3.CompanyId = comp1.Id;
            emp4.CompanyId = comp2.Id;
            emp5.CompanyId = comp3.Id;

            emp2.ManagerId = emp1.Id;
            emp3.ManagerId = emp1.Id;

            emp5.ManagerId = emp4.Id;

            modelBuilder.Entity<Company>().HasData(
                comp1,
                comp2,
                comp3);

            modelBuilder.Entity<Employee>().HasData(
                emp1,
                emp2,
                emp3,
                emp4,
                emp5);

            modelBuilder.Entity<Company>().HasMany(x => x.Employees).WithOne(x => x.Company!);
            modelBuilder.Entity<Employee>().HasMany(x => x.Minions).WithOne(x => x.Manager!);
        }
    }
}
