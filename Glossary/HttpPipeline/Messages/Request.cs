using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace HttpPipeline.Messages;

public class Request
{
    private Dictionary<string, object>? _properties;

    internal Request(HttpMethod method, string? requestUri)
    {
        Method = method;
        RequestUri = requestUri;
    }

    protected Dictionary<string, string>? Headers { get; set; }
    protected Dictionary<string, string>? ContentHeaders { get; set; }
    public Type? ResponseType { get; protected set; }
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
    public void SetContentHeader(string name, string value)
    {
        ContentHeaders ??= new();
        if (ContentHeaders.ContainsKey(name))
        {
            ContentHeaders[name] = value;
        }
        else
        {
            ContentHeaders.Add(name, value);
        }
    }

    public void SetContent<T>(T model) => SetContent(JsonConvert.SerializeObject(model), "application/json");

    public void SetContent(string data, string contentType)
    {
        Content = new BinaryData(data);
        SetContentHeader("Content-Type", contentType);
    }

    public bool TryGetProperty<T>(string propertyName, [NotNullWhen(true)] out T? propertyValue)
    {
        if (_properties?.TryGetValue(propertyName, out var value) == true && value is T correctValue)
        {
            propertyValue = correctValue;
            return true;
        }
        else
        {
            propertyValue = default;
            return false;
        }
    }

    public void SetProperty(string propertyName, object value)
    {
        _properties ??= new();
        _properties[propertyName] = value;
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

            if (ContentHeaders != null)
            {
                foreach (var header in ContentHeaders)
                {
                    request.Content.Headers.Add(header.Key, header.Value);
                }
            }
        }

        return request;
    }
}

public class Request<TResponseType> : Request
{
    internal Request(HttpMethod method, string? requestUri) : base(method, requestUri)
    {
        ResponseType = typeof(TResponseType);
    }
}

