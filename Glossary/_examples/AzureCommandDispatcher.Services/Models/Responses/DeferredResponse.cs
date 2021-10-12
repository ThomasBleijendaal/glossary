using AzureCommandDispatcher.Services.Abstractions;

namespace AzureCommandDispatcher.Services.Models.Responses;

public class DeferredResponse : IDeferredResponse
{
    public DeferredResponse(string uri)
    {
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
    }

    public string Uri { get; set; }
}
