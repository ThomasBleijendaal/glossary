/**
 * Gateways are classes that handle communication between the program and an external service. 
 * These classes use an HttpClient to send requests to the external service and have specific
 * needs to communicate to the external service.
 * 
 * The HttpPipelineBuilder builds a HttpPipeline that is loaded with specific policies that 
 * create a stack through which requests are piped. Each policy should be simple and specific,
 * and could be reused between gateways.
 * 
 * TODO: customize IHttpClientFactory
 */

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
