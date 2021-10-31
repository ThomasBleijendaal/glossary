using HttpPipeline.Messages;

namespace HttpPipeline;

public class RequestBuilder : IRequestBuilder
{
    private readonly Uri _baseUri;

    public RequestBuilder(Uri baseUri)
    {
        _baseUri = baseUri;
    }

    public virtual Request CreateRequest(HttpMethod method, string? requestUri)
        => new Request(method, new Uri(_baseUri, requestUri).ToString());

    public virtual Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string? requestUri)
        where TResponseModel : class
            => new Request<TResponseModel>(method, new Uri(_baseUri, requestUri).ToString());

    public virtual HttpMessage CreateMessage(Request request)
        => new HttpMessage(request, new ResponseClassifier());
}
