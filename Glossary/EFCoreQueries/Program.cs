using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFCoreQueries
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<App>();

            services.AddDbContext<AppDbContext>(
                options => options
                    .UseLoggerFactory(MyLoggerFactory)
                    // D-2: .UseLazyLoadingProxies()
                    .UseSqlServer("server=localhost\\sqlexpress;database=efquery;integrated security=true;"));


            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<App>();

            await app.RunAsync();
        }

        public class App
        {
            private readonly IServiceProvider _serviceProvider;

            public App(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task RunAsync()
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await context.Database.EnsureCreatedAsync();
                    await context.Database.MigrateAsync();
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var emp2 = await context.Employees.FirstAsync(x => x.Id == 2);

                    Console.WriteLine(emp2.Manager == null ? "A: Manager of Employee 2 is null" : "Manager of Employee 2 is not null");

                    // exibit A: what happend to manager of emp2 after this line?
                    var emp1 = await context.Employees.FirstAsync(x => x.Id == 1);

                    Console.WriteLine(emp2.Manager == null ? "A: Manager of Employee 2 is null" : "Manager of Employee 2 is not null");

                    var emp3 = await context.Employees.FirstAsync(x => x.Id == 3);
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var emp1a = await context.Employees.FirstAsync(x => x.Id == 1);

                    Console.WriteLine(emp1a.Company == null ? "B: Company of Employee 1 is null" : "Company of Employee 1 is not null");

                    var emp1b = await context.Employees.Include(x => x.Company).FirstAsync(x => x.Id == 1);

                    Console.WriteLine(emp1b.Company == null ? "B: Company of Employee 1 is null" : "Company of Employee 1 is not null");
                    
                    // exibit B: emp1c.Company was not included but does it have a value after this line?
                    var emp1c = await context.Employees.FirstAsync(x => x.Id == 1);

                    Console.WriteLine(emp1c.Company == null ? "B: Company of Employee 1 is null" : "Company of Employee 1 is not null");
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // exibit C: what is the sql that is generated for this include?
                    var emps = await context.Employees.Include(x => x.Company).ToListAsync();
                    foreach (var emp in emps)
                    {
                        Console.WriteLine(emp.Company == null ? $"C: Company of Employee {emp.Id} is null" : $"Company of Employee {emp.Id} is not null");
                    }
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var emp1 = await context.Employees.FirstAsync(x => x.Id == 1);

                    // exibit D-1: if Minions are not included then they can be added lateron: https://docs.microsoft.com/en-us/ef/core/querying/related-data/explicit
                    await context.Entry(emp1).Collection(x => x.Minions).LoadAsync();

                    // exibit D-2: if .UseLazyLoadingProxies() is used then a this query is added when this property is accessed

                    foreach (var minion in emp1.Minions)
                    {
                        Console.WriteLine($"D: Minion {minion.Id}");
                    }
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var projection = await context.Employees.Select(x => new { x.Id, ManagerName = x.Manager == null ? null : x.Manager.Name }).ToListAsync();

                    foreach (var emp in projection)
                    {
                        Console.WriteLine($"E: Manager of Employee {emp.Id} is {emp.ManagerName}");
                    }
                }
            }
        }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
