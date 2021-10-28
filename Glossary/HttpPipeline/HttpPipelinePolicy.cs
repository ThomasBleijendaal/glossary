using HttpPipeline.Messages;

namespace HttpPipeline;

public abstract class HttpPipelinePolicy 
{
    public abstract Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next);

    public static async Task ProcessNextAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
    {
        if (pipeline.IsEmpty)
        {
            return;
        }

        var policy = pipeline.Span[0];
        var nextPolicies = pipeline[1..];

        await policy.ProcessAsync(message, nextPolicies, () => ProcessNextAsync(message, nextPolicies));
    }
}
