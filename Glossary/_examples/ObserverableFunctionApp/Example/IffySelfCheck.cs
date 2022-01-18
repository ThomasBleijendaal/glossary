using System;
using System.Threading.Tasks;
using ObserverableFunctionApp.AppTest;

namespace ObserverableFunctionApp.Example
{
    public class IffySelfCheck : ISelfCheck
    {
        public Task<Status> CheckAsync()
        {
            if (new Random().NextDouble() < 0.5)
            {
                return Task.FromResult(new Status("iffy", CheckStatus.Healty, CheckType.Internal, DateTime.UtcNow, "A-OK"));
            }
            else if (new Random().NextDouble() < 0.75)
            {
                return Task.FromResult(new Status("iffy", CheckStatus.Degraded, CheckType.Internal, DateTime.UtcNow, "Somewhat borked"));
            }
            else
            {
                return Task.FromResult(new Status("iffy", CheckStatus.Unhealthy, CheckType.Internal, DateTime.UtcNow, "Totally borked"));
            }
        }
    }
}
