using InProcessFunctionApp.TimerFunction;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(TimerFunctionStartup))]

namespace InProcessFunctionApp.TimerFunction;

internal class TimerFunctionStartup : IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        builder.Services.AddSingleton<IFunctionProvider, TimerFunctionProvider>();
    }
}

