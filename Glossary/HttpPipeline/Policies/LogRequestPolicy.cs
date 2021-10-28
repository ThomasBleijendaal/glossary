using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class LogRequestPolicy : HttpPipelinePolicy
{
    private readonly ILogger _logger;

    public LogRequestPolicy(ILogger logger)
    {
        _logger = logger;
    }

    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        var requestBody = message.HttpRequestMessage.Content is HttpContent content 
            ? await content.ReadAsStringAsync() 
            : default;

        _logger.LogInformation("Request body was: {requestBody}", requestBody);

        await next();
    }
}
