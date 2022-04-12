using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace InProcessFunctionApp.TimerFunction;

internal class TimerFunction
{
    public static void Run(TimerInfo timer, ILogger log) => log.LogInformation(string.Join(", ", timer.Schedule.GetNextOccurrences(5)));
}
