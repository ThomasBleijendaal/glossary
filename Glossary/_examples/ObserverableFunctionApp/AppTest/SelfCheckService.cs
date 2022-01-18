using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObserverableFunctionApp.AppTest;

public class SelfCheckService : ISelfCheckService
{
    private readonly IDictionary<ISelfCheck, StatusHistory> _selfCheckHistory;

    public SelfCheckService(
        IEnumerable<ISelfCheck> selfChecks)
    {
        _selfCheckHistory = selfChecks.ToDictionary(
            x => x,
            x => new StatusHistory(
                new Status(x.GetType().Name, CheckStatus.Degraded, CheckType.Internal, DateTime.UtcNow, "Failed to get status"),
                new List<Status>()));
    }

    public IReadOnlyList<StatusHistory> CurrentStatus()
    {
        return _selfCheckHistory.Values.ToList();
    }

    public async Task PerformAllSelfChecksAsync()
    {
        foreach (var selfCheck in _selfCheckHistory)
        {
            var currentCheck = selfCheck.Value;

            var status = await selfCheck.Key.CheckAsync();

            if (status.CheckStatus != CheckStatus.Healty)
            {
                if (currentCheck.History.Count > 0)
                {
                    var latestHistory = currentCheck.History[0];

                    if (latestHistory != status)
                    {
                        currentCheck.History.Insert(0, status);
                    }

                    if (currentCheck.History.Count > 10)
                    {
                        currentCheck.History.Take(10..).ToList().ForEach(x => currentCheck.History.Remove(x));
                    }
                }
                else
                {
                    currentCheck.History.Add(status);
                }
            }

            _selfCheckHistory[selfCheck.Key] = currentCheck with
            {
                CurrentStatus = status
            };
        }
    }
}
