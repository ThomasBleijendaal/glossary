namespace HttpPipeline.Messages;

public class Response
{
    internal Response(HttpResponseMessage? response)
    {
        HttpResponseMessage = response;
    }

    public Response(object? content)
    {
        Content = content;
    }

    public HttpResponseMessage? HttpResponseMessage { get; }

    public object? Content { get; set; }
}

public class Response<TResponseModel> : Response
    where TResponseModel : class
{
    private readonly Response _response;

    public Response(Response response) : base(response.HttpResponseMessage)
    {
        _response = response;
    }

    public new TResponseModel? Content => _response.Content as TResponseModel;
}
