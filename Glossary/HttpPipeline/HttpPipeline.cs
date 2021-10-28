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

    public async Task<Response<TResponseModel>> SendAsync<TResponseModel>(Request<TResponseModel> request)
        where TResponseModel : class
    {
        await HttpPipelinePolicy.ProcessNextAsync(request, _policies);
        return new Response<TResponseModel>(request.Response);
    }
}
