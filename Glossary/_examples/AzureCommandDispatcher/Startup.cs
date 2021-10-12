using Azure.Messaging.ServiceBus;
using AzureCommandDispatcher;
using AzureCommandDispatcher.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureCommandDispatcher;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        builder.Services.AddSingleton(new ServiceBusClient(config["ServiceBus"]));
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(config["Redis"]));

        DependencyConfiguration.Register(builder.Services);
    }
}
