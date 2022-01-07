using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ObserverableFunctionApp.AppTest;

namespace ObserverableFunctionApp
{
    public class ApplicationTestFunction
    {
        private readonly IEnumerable<ISelfCheck> _selfChecks;
        private readonly ILogger<ApplicationTestFunction> _logger;

        public ApplicationTestFunction(
            IEnumerable<ISelfCheck> selfChecks,
            ILogger<ApplicationTestFunction> logger)
        {
            _selfChecks = selfChecks;
            _logger = logger;
        }

        [FunctionName("ApplicationTestFunction")]
        [OpenApiOperation(operationId: "ApplicationTestFunction", tags: new[] { "greetings" })]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StatusResponseModel))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var list = new List<(Status status, IEnumerable<Status> history)>();

            foreach (var selfCheck in _selfChecks)
            {
                var status = await selfCheck.CheckAsync();

                list.Add((status, selfCheck.GetHistory()));
            }

            return new OkObjectResult(new StatusResponseModel(list));
        }
    }
}

namespace ObserverableFunctionApp.AppTest
{ 
    public interface ISelfCheck
    {
        Task<Status> CheckAsync();

        IEnumerable<Status> GetHistory();
    }

    public enum CheckStatus
    {
        Healty,
        Degraded,
        Unhealthy
    }

    public enum CheckType
    {
        Internal,
        External
    }

    public record Status(
        string Name,
        CheckStatus CheckStatus,
        CheckType CheckType,
        DateTime DateTime,
        params string[] Message);

    public class StatusResponseModel
    {
        public StatusResponseModel(IEnumerable<(Status status, IEnumerable<Status> history)> data)
        {
            Internal = GetStatus(data.Select(x => x.status).Where(x => x.CheckType == CheckType.Internal));
            External = GetStatus(data.Select(x => x.status).Where(x => x.CheckType == CheckType.External));
            History = GetHistory(data.SelectMany(x => x.history).GroupBy(x => x.Name));
            Status = new OveralStatus
            {
                AllOk = OkString(data),
                ExternalOk = OkString(data.Where(x => x.status.CheckType == CheckType.External)),
                InternalOk = OkString(data.Where(x => x.status.CheckType == CheckType.Internal))
            };
        }

        public IReadOnlyDictionary<string, string> Internal { get; set; }
        public IReadOnlyDictionary<string, string> External { get; set; }
        public IReadOnlyDictionary<string, IEnumerable<string>> History { get; set; }
        public OveralStatus Status { get; set; }

        public class OveralStatus
        {
            public string AllOk { get; set; }
            public string ExternalOk { get; set; }
            public string InternalOk { get; set; }
        }

        private IReadOnlyDictionary<string, string> GetStatus(IEnumerable<Status> status)
            => status.ToDictionary(x => x.Name, x => $"{x.CheckStatus}: {string.Join(",", x.Message)}");
        private IReadOnlyDictionary<string, IEnumerable<string>> GetHistory(IEnumerable<IGrouping<string, Status>> history)
            => history.ToDictionary(x => x.Key, x => x.Select(y => $"{y.CheckStatus}: {y.DateTime:s}: {string.Join(",", y.Message)}"));

        private static string OkString(IEnumerable<(Status status, IEnumerable<Status> history)> data) 
            => data.All(x => x.status.CheckStatus == CheckStatus.Healty) ? "OK" : "NOT-OK";
    }

    public abstract class SelfCheckBase : ISelfCheck
    {
        // TODO: this should not be here
        private readonly List<Status> _statusHistory = new();

        public async Task<Status> CheckAsync()
        {
            var status = await CheckStatusAsync();

            if (status.CheckStatus != CheckStatus.Healty)
            {
                if (_statusHistory.Count > 10)
                {
                    _statusHistory.Take(10..).ToList().ForEach(x => _statusHistory.Remove(x));
                }

                _statusHistory.Insert(0, status);
            }

            return status;
        }

        public IEnumerable<Status> GetHistory() => _statusHistory;

        protected abstract Task<Status> CheckStatusAsync();
    }

    public class IffySelfCheck : SelfCheckBase
    {
        public IffySelfCheck()
        {
        }

        protected override Task<Status> CheckStatusAsync()
        {
            if (new Random().NextDouble() < 0.5)
            {
                return Task.FromResult(new Status("iffy", CheckStatus.Healty, CheckType.Internal, DateTime.UtcNow));
            }
            else if (new Random().NextDouble() < 0.6)
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
