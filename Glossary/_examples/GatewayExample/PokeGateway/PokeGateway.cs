using HttpPipeline;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GatewayExample.PokeGateway;

public class PokeGateway
{
    private readonly IHttpPipeline _httpPipeline;

    public PokeGateway(
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory, 
        ILogger<PokeGateway> logger)
    {
        var options = new ClientOptions(new Uri("https://pokeapi.co/api/v2/"), httpClientFactory, logger)
        {
            LogRequests = true,
            LogResponses = true,
            Retry = 
            {
                MaxRetries = 1
            }
        };

        options.AddPolicy(HttpPipelinePosition.PerCall, new ResponseMemoryCachePolicy(memoryCache));

        _httpPipeline = HttpPipelineBuilder.Build(options);
    }

    public async Task<PokeResponse?> GetPokeAsync(string name)
    {
        var request = _httpPipeline.CreateRequest<PokeResponse>(HttpMethod.Get, $"pokemon/{name}");

        request.EnsureSuccessStatusCode = true;

        var response = await _httpPipeline.SendAsync(request);
        return response.Content;
    }
}
