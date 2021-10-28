using System.Net.Http.Headers;
using HttpPipeline.Messages;

namespace HttpPipeline.GatewayExamples.AuthenticatedGateway;

public class BearerTokenPolicy : HttpPipelinePolicy
{
    public BearerTokenPolicy(string username, string password)
    {
        // these parameters come from config / keyvault / etc
    }

    public override async Task ProcessAsync(Request message, ReadOnlyMemory<HttpPipelinePolicy> pipeline, Func<Task> next)
    {
        // requests a bearer token from external service

        message.HttpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "ey.ey.SIG");

        await next();
    }
}
