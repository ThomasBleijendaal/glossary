using HttpPipeline.Messages;

namespace HttpPipeline;

public delegate Task NextPolicy();

public interface IHttpPipelinePolicy
{

    public abstract Task ProcessAsync(HttpMessage message, NextPolicy next);

    public static async Task ProcessNextAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline)
    {
        if (pipeline.IsEmpty)
        {
            return;
        }

        var nextPolicies = pipeline[1..];
        await pipeline.Span[0].ProcessAsync(message, () => ProcessNextAsync(message, nextPolicies));
    }
}
