namespace HttpPipeline.Messages;

public class TypedRequest : Request
{
    private readonly Request _request;

    public TypedRequest(Request request, Type responseType) : base(request.HttpRequestMessage)
    {
        _request = request;
        ResponseType = responseType;
    }

    public Type ResponseType { get; }
}
