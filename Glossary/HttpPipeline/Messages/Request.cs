namespace HttpPipeline.Messages;

public class Request
{
    private Response? _response;

    protected Request(HttpRequestMessage httpRequestMessage)
    {
        HttpRequestMessage = httpRequestMessage;
    }

    public HttpRequestMessage HttpRequestMessage;

    public Request(HttpMethod method, string? requestUri)
    {
        HttpRequestMessage = new HttpRequestMessage(method, requestUri);
    }

    public Response Response
    {
        get => _response ?? throw new InvalidOperationException();
        set => _response = value;
    }
}
