using System.Text;
using HttpPipeline;
using HttpPipeline.Messages;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayExample.PokeGateway;

internal class ResponseMemoryCachePolicy : IHttpPipelinePolicy
{
    private readonly IMemoryCache _memoryCache;

    public ResponseMemoryCachePolicy(
        IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        if (_memoryCache.TryGetValue(CacheKey(message.Request), out var value) &&
            value is not null &&
            value.GetType() == message.Request.ResponseType)
        {
            message.Response = new Response(value);

            // stop processing when cache is found
            return;
        }

        await next();

        if (message.Response.Content != null && message.Response.HttpResponseMessage?.IsSuccessStatusCode == true)
        {
            _memoryCache.Set(
                CacheKey(message.Request),
                message.Response.Content,
                TimeSpan.FromMinutes(1));
        }
    }

    private string CacheKey(Request request) => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{request.Method}--{request.RequestUri}"));
}
