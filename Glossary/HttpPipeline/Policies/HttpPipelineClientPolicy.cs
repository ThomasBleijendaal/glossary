using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class HttpPipelineClientPolicy : IHttpPipelinePolicy
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpPipelineClientPolicy(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        var request = message.Request.GetHttpRequestMessage();

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.SendAsync(request);

        message.Response = new Response(response);

        await next();
    }
}
