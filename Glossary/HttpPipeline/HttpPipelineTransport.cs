namespace HttpPipeline;

public class HttpPipelineTransport
{
    private readonly HttpClient? _httpClient;
    private readonly IHttpClientFactory? _httpClientFactory;

    internal HttpPipelineTransport(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public HttpPipelineTransport(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual HttpClient CreateClient()
        => _httpClient ?? _httpClientFactory?.CreateClient() ?? throw new InvalidOperationException("No HttpClient available");
}
