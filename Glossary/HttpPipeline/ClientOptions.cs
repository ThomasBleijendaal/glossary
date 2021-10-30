using Microsoft.Extensions.Logging;

namespace HttpPipeline;

public class ClientOptions
{
    public ClientOptions(
        Uri baseUri,
        IHttpClientFactory httpClientFactory,
        ILogger logger)
    {
        BaseUri = baseUri;
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        Logger = logger;
    }

    internal List<(HttpPipelinePosition Position, IHttpPipelinePolicy Policy)>? Policies { get; private set; }

    public Uri BaseUri { get; }

    public IHttpClientFactory HttpClientFactory { get; }

    public ILogger Logger { get; }

    public IRequestBuilder? RequestBuilder { get; set; }

    public bool LogRequests { get; set; }
    public bool LogResponses { get; set; }

    public int Retries { get; set; }

    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    public bool EnableEnsureSuccessStatusCode { get; set; } = true;

    public void AddPolicy(HttpPipelinePosition position, IHttpPipelinePolicy policy)
    {
        Policies ??= new();
        Policies.Add((position, policy));
    }
}
