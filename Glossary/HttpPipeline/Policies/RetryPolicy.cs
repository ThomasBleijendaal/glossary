using System.Runtime.ExceptionServices;
using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class RetryPolicy : HttpPipelinePolicy
{
    private readonly ILogger _logger;
    private readonly int _maxRetries;

    public RetryPolicy(ILogger logger, int maxRetries)
    {
        _logger = logger;
        _maxRetries = maxRetries;
    }

    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        var attempt = 0;
        List<Exception>? exceptions = null;
        do
        {
            try
            {
                attempt++;

                await next();

                // probably best to check for response code, not all successful request are OK

                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Attempt {attempt} failed", attempt);

                exceptions ??= new();

                exceptions.Add(ex);
            }

            if (attempt > _maxRetries)
            {
                if (exceptions.Count == 1)
                {
                    ExceptionDispatchInfo.Capture(exceptions[0]);
                }
                else
                {
                    throw new AggregateException($"Request failed after {attempt} tries.", exceptions);
                }
            }
        }
        while (true);
    }
}
