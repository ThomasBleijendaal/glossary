using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpPipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ObserverableFunctionApp.Services;

public class Service : ITransientService, IScopedService, ISingletonService
{
    private readonly ILogger<Service> _logger;
    private readonly IHttpPipeline _httpPipeline;

    public Service(ILogger<Service> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;

        var options = new ClientOptions(new Uri("http://localhost:7071"), httpClientFactory, _logger);
        options.AddPolicy(HttpPipelinePosition.PerCall, new TraceIdentifierPolicy(httpContextAccessor));

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task DoSometingAsync()
    {
        await Task.Delay(100);

        if (Random.Shared.NextDouble() < 0.25)
        {
            await _httpPipeline.SendAsync(_httpPipeline.CreateRequest(HttpMethod.Get, "/api/LogFunction"));
        }

        _logger.LogInformation("I'm a service");
    }
}
