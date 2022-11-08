using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class LogRequestPolicy : IHttpPipelinePolicy
{
    private readonly ILogger _logger;

    public LogRequestPolicy(ILogger logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(HttpMessage message, NextPolicy next)
    {
        // it is important to redact any PII here in real world scenarios

        var requestBody = message.Request.Content is BinaryData content 
            ? content.ToString() 
            : default;

        _logger.LogInformation("Request body was: {requestBody}", requestBody);

        // more request details can be logged, like headers and stuff

        return next();
    }
}
