using HttpPipeline.Messages;
using HttpPipeline.Policies;
using Microsoft.Extensions.Logging;

namespace HttpPipeline.GatewayExamples.PokeGateway;

public class PokeGateway
{
    private readonly HttpPipeline _httpPipeline;

    public PokeGateway(IHttpClientFactory httpClientFactory, ILogger<PokeGateway> logger)
    {
        var options = new ClientOptions(httpClientFactory, logger)
        {
            LogRequests = true,
            LogResponses = true,
            Retries = 1
        };

        options.AddPolicy(HttpPipelinePosition.AfterHttpClient, new ParseBodyAsJsonPolicy());

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task<PokeResponse?> GetPokeAsync(string name)
    {
        var request = new Request<PokeResponse>(HttpMethod.Get, $"https://pokeapi.co/api/v2/pokemon/{name}")
        {
            EnsureSuccessStatusCode = true
        };

        var response = await _httpPipeline.SendAsync(request);
        return response.Content;
    }
}
