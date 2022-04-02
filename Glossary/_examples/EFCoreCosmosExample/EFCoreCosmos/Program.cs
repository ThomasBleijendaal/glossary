using EFCoreCosmos.Data;
using EFCoreCosmos.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await new HostBuilder()
    .ConfigureAppConfiguration(config =>
    {
        
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddDbContext<CosmosDbContext>(options =>
        {
            options.UseCosmos(
                "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", 
                "profiles");
        });

        services.AddHostedService<ProfileService>();
    })
    .Build()
    .RunAsync();
