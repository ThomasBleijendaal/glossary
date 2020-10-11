using System;
using System.Threading.Tasks;
using EFDapper.Core.Abstractions;
using EFDapper.Core.Services;
using EFDapper.Repositories;
using EFDapper.Repositories.Abstractions.Repositories;
using EFDapper.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFDapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<App>();

            services.AddDbContext<AppDbContext>(
                options => options
                    .UseLoggerFactory(MyLoggerFactory)
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFDapper"));

            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<App>();

            await app.RunAsync();
        }

        public class App
        {
            private readonly IPersonService _personService;

            public App(IPersonService personService)
            {
                _personService = personService;
            }

            public async Task RunAsync()
            {
                var people1 = await _personService.GetAllPeopleAsync();

                await _personService.UpdatePersonNameAsync(1, $"Test Person {DateTime.Now}");

                var people2 = await _personService.GetAllPeopleAsync();
                ;
            }
        }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
    }
}
