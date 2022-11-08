namespace MessageHandlerMiddleware;

public class DebugPolicy : IHandlerPolicy
{
    public async Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> next)
    {
        var requestContent = request.Content == null ? null : await request.Content.ReadAsStringAsync();

        if (requestContent is not null)
        {
            Console.WriteLine($"> {requestContent}");
        }

        var response = await next();

        var responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"< {responseContent}");

        return response;
    }
}
