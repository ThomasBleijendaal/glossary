using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class LogResponsePolicy : IHttpPipelinePolicy
{
    private readonly ILogger _logger;

    public LogResponsePolicy(ILogger logger)
    {
        _logger = logger;
    }

    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        await next();

        // it is important to redact any PII here in real world scenarios

        var responseBody = message.Response.HttpResponseMessage.Content is HttpContent content
            ? await content.ReadAsStringAsync()
            : default;

        _logger.LogInformation("Response body was: {responseBody}", responseBody);

        // more response details can be logged, like headers and stuff
    }
}
