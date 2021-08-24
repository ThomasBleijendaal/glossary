using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ZCommon;

namespace Repository
{
    /**
     * Repositories provide an abstraction of data, removing the dependency of an application to a certain data store
     * (EF Core, MongoDB) and putting it behind an interface. The repository is responsible for CRUD like interactions
     * with the data, as well as executing complex queries.
     * 
     * A repository should never return it's entities, but should always convert them to a model.
     * 
     * Since it's often more efficient to run certain queries and projections in the data store, instead of in-memory,
     * repositories have the tendency to become cluttered with lots of methods, doing the same thing slightly differently.
     * When a service is employed to wrap around a repository, the service usually becomes nothing more than a proxy for
     * all the different types of methods that exist on the repository.
     * 
     * By generalizing the repository to only basic stuff, like getting one entity, getting a list, updating one entity,
     * deleting one entity, or inserting one entity, the repository can be made generic, as these interactions are similar
     * for all types of entities.
     * 
     * Extending this repository with the specification pattern, the repository can execute complex queries and projections
     * without becoming cluttered and messy. The specifications do not depend on a specific data store, making them very
     * unit testable. Since they exist as seperate classes, one can extend and differentiate lots of different specifications
     * while remaining DRY.
     */

    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, RepositoryApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddDbContext<ExampleDbContext>(config => config.UseInMemoryDatabase("example"));

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IMapper<CompanyEntity, BasicCompanyModel>, CompanyMapper>();

            services.AddScoped<ICompanyService, CompanyService>();

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ExampleDbContext>();

            dbContext.Companies.Add(new CompanyEntity { Name = "Company 1" });

            dbContext.Companies.Add(new CompanyEntity { Name = "Company 1a", ParentCompanyId = 1 });
            dbContext.Companies.Add(new CompanyEntity { Name = "Company 1b", ParentCompanyId = 1 });
            dbContext.Companies.Add(new CompanyEntity { Name = "Company 1b1", ParentCompanyId = 3 });

            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 1", CompanyId = 1 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 2", CompanyId = 1 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 3", CompanyId = 2 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 4", CompanyId = 3 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 5", CompanyId = 4 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 6", CompanyId = 4 });
            dbContext.Employees.Add(new EmployeeEntity { Name = "Employee 7", CompanyId = 4 });

            dbContext.SaveChanges();
        }

        public class RepositoryApp : BaseApp
        {
            private readonly ICompanyService _companyService;

            public RepositoryApp(ICompanyService companyService)
            {
                _companyService = companyService;
            }

            public override async Task Run()
            {
                var company1 = await _companyService.GetCompanyAsync(1);

                Console.WriteLine(JsonConvert.SerializeObject(company1, Formatting.Indented));

                Console.WriteLine("");

                var company2Details = await _companyService.GetCompanyDetailsAsync(2);

                Console.WriteLine(JsonConvert.SerializeObject(company2Details, Formatting.Indented));

                Console.WriteLine("");

                var companyHierarchies = await _companyService.GetAllCompanyHierarchies();

                Console.WriteLine(JsonConvert.SerializeObject(companyHierarchies, Formatting.Indented));

            }
        }
    }
}
