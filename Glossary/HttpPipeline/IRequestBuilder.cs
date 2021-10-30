using HttpPipeline.Messages;

namespace HttpPipeline;

public interface IRequestBuilder
{
    Request CreateRequest(HttpMethod method, string? requestUri);
    Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string? requestUri)
        where TResponseModel : class;
    HttpMessage CreateMessage(Request request);
}
