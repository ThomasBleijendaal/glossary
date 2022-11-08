/**
 * The HttpPipeline shows how to create stack of policies that behave as a middleware layer.
 * But sometimes, when using an SDK for example, it is impossible to use a custom Http Pipeline.
 * This example shows how to extend the HttpClientHandler to accept an array of policies, so that
 * it is still possible to compose a series of policies, but the middleware is now packaged inside
 * a perfectly normal HttpClient.
 * 
 * This implementation shows the concept, one should use https://github.com/App-vNext/Polly when
 * implementing it for real code.
 */

using System.Text;
using MessageHandlerMiddleware;

var handler = new MiddlewareHandler(new IHandlerPolicy[]
{
    new TokenPolicy(),
    new DebugPolicy()
});

var client = new HttpClient(handler);

await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://httpbin.org/anything")
{
    Content = new  StringContent($$"""
        {
            "request": "body"
        }
        """, Encoding.UTF8, "application/json")
});

