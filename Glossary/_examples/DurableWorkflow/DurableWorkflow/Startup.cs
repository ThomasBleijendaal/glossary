using DurableWorkflowExample;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DurableWorkflowExample;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddTransient(typeof(IWorkflowOrchestrator<,,>), typeof(WorkflowOrchestrator<,,>));
        builder.Services.AddTransient<IWorkflowMonitor, WorkflowMonitor>();

        builder.Services.AddTransient<ExampleWorkflow>();

        builder.Services.AddTransient<IService, Service>();

        builder.Services.AddLogging(config => config.AddConsole());
    }
}
