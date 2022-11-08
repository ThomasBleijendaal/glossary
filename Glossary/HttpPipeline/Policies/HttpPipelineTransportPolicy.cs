using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class HttpPipelineTransportPolicy : IHttpPipelinePolicy
{
    private readonly HttpPipelineTransport _httpPipelineTransport;

    public HttpPipelineTransportPolicy(HttpPipelineTransport httpPipelineTransport)
    {
        _httpPipelineTransport = httpPipelineTransport;
    }

    public async Task ProcessAsync(HttpMessage message, NextPolicy next)
    {
        message.Response = await _httpPipelineTransport.SendAsync(message);
    }
}
