namespace HttpPipeline.Messages;

public class Request
{
    private Response? _response;

    public Request(HttpMethod method, string? requestUri)
    {
        Method = method;
        RequestUri = requestUri;
    }

    public Response Response
    {
        get => _response ?? throw new InvalidOperationException();
        set => _response = value;
    }

    protected Dictionary<string, string>? Headers { get; set; }
    internal Type? ResponseType { get; set; }
    public BinaryData? Content { get; protected set; }

    public HttpMethod Method { get; }
    public string? RequestUri { get; }
    public bool EnsureSuccessStatusCode { get; set; }

    public void SetHeader(string name, string value)
    {
        Headers ??= new();
        if (Headers.ContainsKey(name))
        {
            Headers[name] = value;
        }
        else
        {
            Headers.Add(name, value);
        }
    }

    public void SetContent<T>(T model)
    {
        Content = new BinaryData(model);
        SetHeader("Content-Type", "application/json");
    }

    internal HttpRequestMessage GetHttpRequestMessage()
    {
        var request = new HttpRequestMessage(Method, RequestUri);

        if (Headers != null)
        {
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        if (Content != null)
        {
            request.Content = new StreamContent(Content.ToStream());
        }

        return request;
    }
}

public class Request<TResponseType> : Request
{
    public Request(HttpMethod method, string? requestUri) : base(method, requestUri)
    {
        ResponseType = typeof(TResponseType);
    }
}

