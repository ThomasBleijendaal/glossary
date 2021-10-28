using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class LogResponsePolicy : HttpPipelinePolicy
{
    private readonly ILogger _logger;

    public LogResponsePolicy(ILogger logger)
    {
        _logger = logger;
    }

    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        var responseBody = message.Response.HttpResponseResponse.Content is HttpContent content
            ? await content.ReadAsStringAsync()
            : default;

        _logger.LogInformation("Response body was: {responseBody}", responseBody);

        await next();
    }
}
