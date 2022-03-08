using DurableWorkflow;
using DurableWorkflowExample;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DurableWorkflow
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient(typeof(IWorkflowOrchestrator<,,>), typeof(WorkflowOrchestrator<,,>));

            builder.Services.AddTransient<ExampleWorkflow>();

            builder.Services.AddTransient<IService, Service>();
        }
    }
}
