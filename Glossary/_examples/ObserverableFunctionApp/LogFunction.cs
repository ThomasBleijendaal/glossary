﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ObserverableFunctionApp.Extensions;
using ObserverableFunctionApp.Services;

namespace ObserverableFunctionApp;

public class LogFunction
{
    private readonly ILogger<LogFunction> _logger;
    private readonly ITransientService _transientService;
    private readonly IScopedService _scopedService;
    private readonly ISingletonService _singletonService;

    public LogFunction(
        ILogger<LogFunction> logger,
        ITransientService transientService,
        IScopedService scopedService,
        ISingletonService singletonService)
    {
        _logger = logger;
        _transientService = transientService;
        _scopedService = scopedService;
        _singletonService = singletonService;
    }

    [FunctionName("LogFunction")]
    [OpenApiOperation(operationId: "LogFunction", tags: new[] { "greetings" })]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(string))]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        using var scope = _logger.BeginHttpContextScope(req.HttpContext);

        _logger.LogInformation("Got a request!");

        using (var logScope = _logger.BeginScope("Interesting {state}", 123))
        {
            _logger.LogInformation("Doing something fancy");

            await _transientService.DoSometingAsync();

            using (var deepLogScope = _logger.BeginScope("Deep {state}", "abc"))
            {
                _logger.LogInformation("Doing something fancy DEEEEP");

                await _scopedService.DoSometingAsync();
            }

            _logger.LogInformation("Doing something fancy again");

            await _singletonService.DoSometingAsync();
        }

        _logger.LogInformation("DONE");

        return new OkObjectResult(req.HttpContext.TraceIdentifier);
    }
}
