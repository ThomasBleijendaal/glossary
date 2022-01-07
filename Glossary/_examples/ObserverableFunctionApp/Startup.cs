using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ObserverableFunctionApp;
using ObserverableFunctionApp.AppTest;
using Serilog;
using Serilog.Core;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ObserverableFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            var logLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
            var logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.Seq("http://localhost:5341")
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Services.AddLogging(lb => lb.AddSerilog(logger));

            builder.Services.AddTransient<ITransientService, Service>();
            builder.Services.AddScoped<IScopedService, Service>();
            builder.Services.AddSingleton<ISingletonService, Service>();

            // wrong lifetime
            builder.Services.AddSingleton<ISelfCheck, IffySelfCheck>();
        }
    }
}
