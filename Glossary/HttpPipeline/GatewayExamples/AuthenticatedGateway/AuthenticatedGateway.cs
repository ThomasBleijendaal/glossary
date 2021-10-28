using HttpPipeline.Messages;
using HttpPipeline.Policies;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.GatewayExamples.AuthenticatedGateway;

public class AuthenticatedGateway
{
    private readonly HttpPipeline _httpPipeline;

    public AuthenticatedGateway(IHttpClientFactory httpClientFactory, ILogger<AuthenticatedGateway> logger)
    {
        var options = new ClientOptions(httpClientFactory, logger);

        options.AddPolicy(HttpPipelinePosition.BeforeHttpClient, new BearerTokenPolicy("username", "password"));
        options.AddPolicy(HttpPipelinePosition.AfterHttpClient, new ParseBodyAsJsonPolicy());

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task<AuthResponse?> GetAuthAsync(string name)
    {
        var request = new Request(HttpMethod.Get, $"https://pokeapi.co/api/v2/pokemon/{name}");
        var response = await _httpPipeline.SendAsync<AuthResponse>(request);
        return response.Content;
    }
}
