using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class HttpPipelineClientPolicy : HttpPipelinePolicy
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpPipelineClientPolicy(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        var request = message.HttpRequestMessage;

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.SendAsync(request);

        message.Response = new Response(response);

        await next();
    }
}
