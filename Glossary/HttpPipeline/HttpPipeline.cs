using HttpPipeline.Messages;

namespace HttpPipeline;

public class HttpPipeline
{
    private readonly ReadOnlyMemory<HttpPipelinePolicy> _policies;

    public HttpPipeline(HttpPipelinePolicy[] policies)
    {
        _policies = policies;
    }

    public async Task<Response> SendAsync(Request request)
    {
        await HttpPipelinePolicy.ProcessNextAsync(request, _policies);
        return request.Response;
    }

    public async Task<Response<TResponseModel>> SendAsync<TResponseModel>(Request request)
        where TResponseModel : class
    {
        var typedRequest = new TypedRequest(request, typeof(TResponseModel));

        await SendAsync(typedRequest);

        return new Response<TResponseModel>(typedRequest.Response);
    }
}
