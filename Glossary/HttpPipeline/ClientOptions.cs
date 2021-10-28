using Microsoft.Extensions.Logging;

namespace HttpPipeline;

public class ClientOptions
{
    public ClientOptions(
        IHttpClientFactory httpClientFactory,
        ILogger logger)
    {
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        Logger = logger;
    }

    internal List<(HttpPipelinePosition Position, HttpPipelinePolicy Policy)>? Policies { get; private set; }

    public IHttpClientFactory HttpClientFactory { get; set; }
    public ILogger Logger { get; }
    public bool LogRequests { get; set; }
    public bool LogResponses { get; set; }

    public void AddPolicy(HttpPipelinePosition position, HttpPipelinePolicy policy)
    {
        Policies ??= new();

        Policies.Add((position, policy));
    }

}
