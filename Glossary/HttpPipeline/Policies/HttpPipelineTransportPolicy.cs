using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class HttpPipelineTransportPolicy : IHttpPipelinePolicy
{
    private readonly HttpPipelineTransport _httpPipelineTransport;

    public HttpPipelineTransportPolicy(HttpPipelineTransport httpPipelineTransport)
    {
        _httpPipelineTransport = httpPipelineTransport;
    }

    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        var request = message.Request.GetHttpRequestMessage();

        var httpClient = _httpPipelineTransport.CreateClient();

        var response = await httpClient.SendAsync(request);

        message.Response = new Response(response);
    }
}
