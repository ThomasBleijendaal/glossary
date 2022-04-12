using InProcessFunctionApp.ReadFile;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(ReadFileStartup))]

namespace InProcessFunctionApp.ReadFile;

internal class ReadFileStartup : IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        builder.AddReadFileBinding();
    }
}

