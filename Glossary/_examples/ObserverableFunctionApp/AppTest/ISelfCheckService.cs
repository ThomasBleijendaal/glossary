using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObserverableFunctionApp.AppTest;

public interface ISelfCheckService
{
    IReadOnlyList<StatusHistory> CurrentStatus();

    Task PerformAllSelfChecksAsync();
}
