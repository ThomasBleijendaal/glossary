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
        Transport = new HttpPipelineTransport(httpClientFactory);
        Logger = logger;
    }

    public ClientOptions(
        Uri baseUri,
        HttpPipelineTransport httpPipelineTransport,
        ILogger logger)
    {
        BaseUri = baseUri;
        Transport = httpPipelineTransport;
        Logger = logger;
    }

    internal List<(HttpPipelinePosition Position, IHttpPipelinePolicy Policy)>? Policies { get; private set; }

    public Uri BaseUri { get; }

    public HttpPipelineTransport Transport { get; }

    public ILogger Logger { get; }

    public IRequestBuilder? RequestBuilder { get; set; }

    public bool LogRequests { get; set; }
    public bool LogResponses { get; set; }

    public RetryOptions Retry { get; set; } = new RetryOptions();

    public bool EnableEnsureSuccessStatusCode { get; set; } = true;

    public bool ParseBodyAsJson { get; set; } = true;

    public void AddPolicy(HttpPipelinePosition position, IHttpPipelinePolicy policy)
    {
        Policies ??= new();
        Policies.Add((position, policy));
    }
}
