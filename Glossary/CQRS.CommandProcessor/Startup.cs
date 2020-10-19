using CQRS.Handlers;
using CQRS.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CQRS.CommandProcessor.Startup))]
namespace CQRS.CommandProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            builder.Services.AddTransient<ICommandHandler, BackgroundCommandHander>();
        }
    }
}
