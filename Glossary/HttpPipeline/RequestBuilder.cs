using HttpPipeline.Messages;

namespace HttpPipeline;

internal class RequestBuilder : IRequestBuilder
{
    private readonly Uri _baseUri;

    public RequestBuilder(Uri baseUri)
    {
        _baseUri = baseUri;
    }

    public Request CreateRequest(HttpMethod method, string? requestUri)
        => new Request(method, new Uri(_baseUri, requestUri).ToString());

    public Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string? requestUri)
        where TResponseModel : class
            => new Request<TResponseModel>(method, new Uri(_baseUri, requestUri).ToString());

    public HttpMessage CreateMessage(Request request)
        => new HttpMessage(request, new ResponseClassifier());
}
