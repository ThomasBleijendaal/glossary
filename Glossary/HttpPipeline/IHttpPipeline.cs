using HttpPipeline.Messages;

namespace HttpPipeline;

public interface IHttpPipeline
{
    Request CreateRequest(HttpMethod method, string requestUri);
    Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string requestUri)
        where TResponseModel : class;
    Task<Response> SendAsync(Request request);
    Task<Response<TResponseModel>> SendAsync<TResponseModel>(Request<TResponseModel> request)
        where TResponseModel : class;
}
