using System.Net;
using HttpPipeline.Messages;

namespace HttpPipeline;

public class ResponseClassifier
{
    public virtual bool IsRetriableResponse(HttpMessage message) 
        => RetriableStatusCode(message.Response.HttpResponseMessage.StatusCode);

    public virtual bool IsRetriableException(Exception ex)
        => ex is IOException || (ex is HttpRequestException httpRequestException && RetriableStatusCode(httpRequestException.StatusCode));

    private static bool RetriableStatusCode(HttpStatusCode? statusCode)
        => statusCode switch
        {
            HttpStatusCode.RequestTimeout or
            HttpStatusCode.TooManyRequests or
            HttpStatusCode.InternalServerError or
            HttpStatusCode.BadGateway or
            HttpStatusCode.ServiceUnavailable or
            HttpStatusCode.GatewayTimeout => true,
            _ => false,
        };
}
