using HttpPipeline;
using HttpPipeline.Policies;
using Microsoft.Extensions.Logging;

namespace GatewayExample.AuthenticatedGateway;

public class AuthenticatedGateway
{
    private readonly IHttpPipeline _httpPipeline;

    public AuthenticatedGateway(IHttpClientFactory httpClientFactory, ILogger<AuthenticatedGateway> logger)
    {
        var options = new ClientOptions(new Uri("https://httppipeline.azurewebsites.net"), httpClientFactory, logger)
        {
            Retries = 10
        };

        options.AddPolicy(HttpPipelinePosition.BeforeHttpClient, new AuthenticatedGatewayBearerTokenPolicy("username", "password"));
        options.AddPolicy(HttpPipelinePosition.AfterHttpClient, new ParseBodyAsJsonPolicy());

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task<AuthResponse?> GetAuthAsync(string name)
    {
        var request = _httpPipeline.CreateRequest<AuthResponse>(HttpMethod.Post, "api/get");

        request.SetContent(new { name });

        var response = await _httpPipeline.SendAsync(request);
        return response.Content;
    }
}
