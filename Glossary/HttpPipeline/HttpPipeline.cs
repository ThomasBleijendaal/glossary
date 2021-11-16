/**
 * Gateways are classes that handle communication between the program and an external service. 
 * These classes use an HttpClient to send requests to the external service and have specific
 * needs to communicate to the external service.
 * 
 * The HttpPipelineBuilder builds a HttpPipeline that is loaded with specific policies that 
 * create a stack through which requests are piped. Each policy should be simple and specific,
 * and could be reused between gateways.
 * 
 * The stack of policies behave as a middleware layer, in which they can modify the request,
 * allow further policies to do their thing, and modify the response if needed. It's even possible
 * to short-circuit the pipeline when some condition is met.
 * 
 * [HttpPipeline]
 *  |
 *  | Sends message to policy stack
 *  |
 *  V
 * [Policy A]
 * - ProcessAsync
 *   - modifies request (optionally)
 *   - calls next() -----> [PolicyB]
 *     |                   - ProcessAsync
 *     |                     - modifies request (optionally)
 *     |                     - calls next() ---------> [HttpPipelineTransportPolicy]
 *     |                       |                       - ProcessAsync
 *     |                       |                         - calls HttpPipelineTransport.SendAsync()
 *     |                       |                         - sets response on HttpMessage
 *     |                       |<------------------------- returns
 *     |                       V
 *     |                     - modifies response (optionally)
 *     |---------------------- returns
 *     V
 *   - modifies response (optionally)
 *   - returns
 * 
 */

using HttpPipeline.Messages;

namespace HttpPipeline;

internal class HttpPipeline : IHttpPipeline
{
    private readonly HttpPipelineTransport _httpPipelineTransport;
    private readonly ReadOnlyMemory<IHttpPipelinePolicy> _policies;

    internal HttpPipeline(
        HttpPipelineTransport httpPipelineTransport,
        IHttpPipelinePolicy[] policies)
    {
        _httpPipelineTransport = httpPipelineTransport;
        _policies = policies;
    }

    public async Task<Response> SendAsync(Request request)
    {
        var message = _httpPipelineTransport.CreateMessage(request);
        await IHttpPipelinePolicy.ProcessNextAsync(message, _policies);
        return message.Response;
    }

    public async Task<Response<TResponseModel>> SendAsync<TResponseModel>(Request<TResponseModel> request)
        where TResponseModel : class
    {
        var message = _httpPipelineTransport.CreateMessage(request);
        await IHttpPipelinePolicy.ProcessNextAsync(message, _policies);
        return new Response<TResponseModel>(message.Response);
    }

    public Request CreateRequest(HttpMethod method, string requestUri)
        => _httpPipelineTransport.CreateRequest(method, requestUri);

    public Request<TResponseModel> CreateRequest<TResponseModel>(HttpMethod method, string requestUri)
        where TResponseModel : class
            => _httpPipelineTransport.CreateRequest<TResponseModel>(method, requestUri);
}
