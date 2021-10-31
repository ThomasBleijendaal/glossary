namespace HttpPipeline.Messages;

public class HttpMessage : IDisposable
{
    private Response? _response;

    public HttpMessage(Request request, ResponseClassifier responseClassifier)
    {
        Request = request ?? throw new ArgumentNullException(nameof(request));
        ResponseClassifier = responseClassifier;
    }

    public Request Request { get; }

    public ResponseClassifier ResponseClassifier { get; }

    public Response Response
    {
        get => _response ?? throw new InvalidOperationException();
        set => _response = value;
    }

    public void Dispose()
    {
        DisposeAndDropResponse();
    }

    public void DisposeAndDropResponse()
    {
        _response?.HttpResponseMessage.Dispose();
        _response = null;
    }
}

