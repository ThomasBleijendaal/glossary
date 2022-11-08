namespace MessageHandlerMiddleware;

public interface IHandlerPolicy
{
    Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> next);
}
