namespace MessageHandlerMiddleware;

public class MiddlewareHandler : HttpClientHandler
{
    private readonly ReadOnlyMemory<IHandlerPolicy> _policies;

    public MiddlewareHandler(
        IHandlerPolicy[] policies)
    {
        _policies = policies;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return ProcessNextAsync(request, cancellationToken, _policies, () => base.SendAsync(request, cancellationToken));
    }

    private static async Task<HttpResponseMessage> ProcessNextAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken, 
        ReadOnlyMemory<IHandlerPolicy> pipeline, 
        Func<Task<HttpResponseMessage>> send)
    {
        if (pipeline.IsEmpty)
        {
            return await send.Invoke();
        }

        var nextPolicies = pipeline[1..];
        
        return await pipeline.Span[0].ProcessAsync(request, cancellationToken, () => ProcessNextAsync(request, cancellationToken, nextPolicies, send));
    }
}
