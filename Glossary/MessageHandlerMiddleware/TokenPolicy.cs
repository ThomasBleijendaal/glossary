using System.Net.Http.Headers;

namespace MessageHandlerMiddleware;

public class TokenPolicy : IHandlerPolicy
{
    public Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> next)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "Token");

        return next.Invoke();
    }
}
