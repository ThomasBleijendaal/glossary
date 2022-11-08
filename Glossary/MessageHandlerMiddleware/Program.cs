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

