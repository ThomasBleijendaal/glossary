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

using HttpPipeline.Messages;

namespace HttpPipeline;

internal class HttpPipeline : IHttpPipeline
{
    private readonly IRequestBuilder _requestBuilder;
    private readonly ReadOnlyMemory<IHttpPipelinePolicy> _policies;

    internal HttpPipeline(
        IRequestBuilder requestBuilder,
        IHttpPipelinePolicy[] policies)
    {
        _requestBuilder = requestBuilder;
        _policies = policies;
    }

    public async Task<Response> SendAsync(Request request)
    {
        var message = _requestBuilder.CreateMessage(request);
        await IHttpPipelinePolicy.ProcessNextAsync(message, _policies);
        return message.Response;
    }

    public async Task<Response<TResponseModel>> SendAsync<TResponseModel>(Request<TResponseModel> request)
        where TResponseModel : class
    {
        var message = _requestBuilder.CreateMessage(request);
        await IHttpPipelinePolicy.ProcessNextAsync(message, _policies);
        return new Response<TResponseModel>(message.Response);
    }

    public Request CreateRequest(HttpMethod method, string requestUri)
        => _requestBuilder.CreateRequest(method, requestUri);

    public Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string requestUri)
        where TResponseModel : class
            => _requestBuilder.CreateRequest<TResponseModel>(method, requestUri);
}
