using HttpPipeline.Messages;
using Newtonsoft.Json;

namespace HttpPipeline.Policies;

public class ParseBodyAsJsonPolicy : HttpPipelinePolicy
{
    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        if (message.ResponseType != null)
        {
            var responseObject = JsonConvert.DeserializeObject(
                await message.Response.HttpResponseMessage.Content.ReadAsStringAsync(),
                message.ResponseType);

            message.Response.Content = responseObject;
        }
        await next();
    }
}
