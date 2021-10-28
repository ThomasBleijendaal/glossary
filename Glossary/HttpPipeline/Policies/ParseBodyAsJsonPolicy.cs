using HttpPipeline.Messages;
using Newtonsoft.Json;

namespace HttpPipeline.Policies;

internal class ParseBodyAsJsonPolicy : HttpPipelinePolicy
{
    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        if (message is TypedRequest typedRequest)
        {
            var responseObject = JsonConvert.DeserializeObject(
                await message.Response.HttpResponseResponse.Content.ReadAsStringAsync(),
                typedRequest.ResponseType);

            message.Response.Content = responseObject;
        }
        await next();
    }
}
