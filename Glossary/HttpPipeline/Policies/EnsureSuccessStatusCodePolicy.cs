using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

internal class EnsureSuccessStatusCodePolicy : IHttpPipelinePolicy
{
    public async Task ProcessAsync(HttpMessage message, NextPolicy next)
    {
        await next();
        
        if (message.Request.EnsureSuccessStatusCode && message.Response.HttpResponseMessage != null)
        {
            message.Response.HttpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
