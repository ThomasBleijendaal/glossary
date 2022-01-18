using System.Collections.Generic;
using System.Linq;

namespace ObserverableFunctionApp.AppTest;

public class StatusResponseModel
{
    public StatusResponseModel(IEnumerable<StatusHistory> history)
    {
        Internal = GetStatus(history.Select(x => x.CurrentStatus).Where(x => x.CheckType == CheckType.Internal));
        External = GetStatus(history.Select(x => x.CurrentStatus).Where(x => x.CheckType == CheckType.External));
        History = GetHistory(history.SelectMany(x => x.History).GroupBy(x => x.Name));
        Status = new OveralStatus
        {
            AllOk = OkString(history),
            ExternalOk = OkString(history.Where(x => x.CurrentStatus.CheckType == CheckType.External)),
            InternalOk = OkString(history.Where(x => x.CurrentStatus.CheckType == CheckType.Internal))
        };
    }

    public IReadOnlyDictionary<string, string> Internal { get; set; }
    public IReadOnlyDictionary<string, string> External { get; set; }
    public IReadOnlyDictionary<string, IEnumerable<string>> History { get; set; }
    public OveralStatus Status { get; set; }

    public class OveralStatus
    {
        public string? AllOk { get; set; }
        public string? ExternalOk { get; set; }
        public string? InternalOk { get; set; }
    }

    private IReadOnlyDictionary<string, string> GetStatus(IEnumerable<Status> status)
        => status.ToDictionary(x => x.Name, x => $"{x.CheckStatus}: {string.Join(",", x.Message)}");

    private IReadOnlyDictionary<string, IEnumerable<string>> GetHistory(IEnumerable<IGrouping<string, Status>> history)
        => history.ToDictionary(x => x.Key, x => x.Select(y => $"{y.CheckStatus}: {y.DateTime:s}: {string.Join(",", y.Message)}"));

    private static string OkString(IEnumerable<StatusHistory> data)
        => data.All(x => x.CurrentStatus.CheckStatus == CheckStatus.Healty) ? "OK" : "NOT-OK";
}
