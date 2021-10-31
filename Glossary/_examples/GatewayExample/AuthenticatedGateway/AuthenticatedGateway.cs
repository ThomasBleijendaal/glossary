using HttpPipeline;
using Microsoft.Extensions.Logging;

namespace GatewayExample.AuthenticatedGateway;

public class AuthenticatedGateway
{
    private readonly IHttpPipeline _httpPipeline;

    public AuthenticatedGateway(IHttpClientFactory httpClientFactory, ILogger<AuthenticatedGateway> logger)
    {
        var options = new ClientOptions(new Uri("https://httppipeline.azurewebsites.net"), httpClientFactory, logger)
        {
            Retry = 
            {
                MaxRetries = 10
            }
        };

        options.AddPolicy(HttpPipelinePosition.BeforeTransport, new AuthenticatedGatewayBearerTokenPolicy("username", "password"));

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task<AuthResponse?> GetAuthAsync(string name)
    {
        var request = _httpPipeline.CreateRequest<AuthResponse>(HttpMethod.Post, "api/get");

        request.SetContent(new { name });
        request.SetProperty(AuthenticatedGatewayBearerTokenPolicy.ScopePropertyKey, "scope");

        var response = await _httpPipeline.SendAsync(request);
        return response.Content;
    }
}
