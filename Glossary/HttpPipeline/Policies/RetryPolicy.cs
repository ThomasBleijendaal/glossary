using System.Runtime.ExceptionServices;
using HttpPipeline.Messages;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.Policies;

internal class RetryPolicy : IHttpPipelinePolicy
{
    private readonly ILogger _logger;
    private readonly RetryOptions _options;

    public RetryPolicy(ILogger logger, RetryOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task ProcessAsync(HttpMessage message, NextPolicy next)
    {
        var attempt = 0;
        List<Exception>? exceptions = null;
        do
        {
            Exception? lastException = null;

            try
            {
                attempt++;
                await next();
            }
            catch (Exception ex)
            {
                if (!message.ResponseClassifier.IsRetriableException(ex))
                {
                    throw;
                }

                _logger.LogWarning(ex, "Attempt {attempt} failed", attempt);

                lastException = ex;
                exceptions ??= new();
                exceptions.Add(ex);
            }

            var shouldRetry = attempt <= _options.MaxRetries;
            var delay = TimeSpan.Zero;

            if (lastException != null)
            {
                if (shouldRetry)
                {
                    delay = _options.RetryDelay;
                }
                else
                {
                    if (exceptions!.Count == 1)
                    {
                        ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
                    }
                    else
                    {
                        throw new AggregateException($"Request failed after {attempt} tries.", exceptions);
                    }
                }
            }
            else if (message.ResponseClassifier.IsRetriableResponse(message))
            {
                if (shouldRetry)
                {
                    delay = _options.RetryDelay;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (delay > TimeSpan.Zero)
            {
                message.DisposeAndDropResponse();

                await Task.Delay(delay);
            }
        }
        while (true);
    }
}
