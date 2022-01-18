using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ObserverableFunctionApp.AppTest;

namespace ObserverableFunctionApp;

public class ApplicationTestFunction
{
    private readonly ISelfCheckService _selfCheckService;
    private readonly ILogger<ApplicationTestFunction> _logger;

    public ApplicationTestFunction(
        ISelfCheckService selfCheckService,
        ILogger<ApplicationTestFunction> logger)
    {
        _selfCheckService = selfCheckService;
        _logger = logger;
    }

    [FunctionName("ApplicationTestFunction")]
    [OpenApiOperation(operationId: "ApplicationTestFunction", tags: new[] { "greetings" })]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(StatusResponseModel))]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        return new OkObjectResult(new StatusResponseModel(_selfCheckService.CurrentStatus()));
    }

    [FunctionName("ApplicationTestTimerFunction")]
    public async Task RunAsync([TimerTrigger("*/1 * * * * *", RunOnStartup = true)] TimerInfo info)
    {
        await _selfCheckService.PerformAllSelfChecksAsync();
    }
}
