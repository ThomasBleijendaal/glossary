using HttpPipeline.Messages;

namespace HttpPipeline;

public class HttpPipelineTransport
{
    private readonly HttpClient? _httpClient;
    private readonly IHttpClientFactory? _httpClientFactory;
    private readonly Uri _baseUri;

    internal HttpPipelineTransport(
        IHttpClientFactory httpClientFactory,
        Uri baseUri)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _baseUri = baseUri;
    }

    public HttpPipelineTransport(
        HttpClient httpClient,
        Uri? baseUri = null)
    {
        _httpClient = httpClient;
        _baseUri = httpClient.BaseAddress ?? baseUri
            ?? throw new ArgumentException("Either HttpClient.BaseAddress or baseUri should not be null");
    }

    protected virtual HttpClient HttpClient
        => _httpClient 
        ?? _httpClientFactory?.CreateClient() 
        ?? throw new InvalidOperationException("No HttpClient available");

    public virtual async Task<Response> SendAsync(HttpMessage message)
    {
        var request = message.Request.GetHttpRequestMessage();
        var response = await HttpClient.SendAsync(request);
        return new Response(response);
    }

    public virtual Request CreateRequest(HttpMethod method, string? requestUri)
        => new Request(method, new Uri(_baseUri, requestUri).ToString());

    public virtual Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string? requestUri)
        where TResponseModel : class
            => new Request<TResponseModel>(method, new Uri(_baseUri, requestUri).ToString());

    public virtual HttpMessage CreateMessage(Request request)
        => new HttpMessage(request, new ResponseClassifier());
}
