using AzureCommandDispatcher.Services.Abstractions;

namespace AzureCommandDispatcher.Services.Models.Responses;

internal class DispatchResponse : IDispatchResponse
{
    public DispatchResponse(IDeferredResponse deferredResponse, Task activityTracker)
    {
        DeferredResponse = deferredResponse ?? throw new ArgumentNullException(nameof(deferredResponse));
        ActivityTracker = activityTracker ?? throw new ArgumentNullException(nameof(activityTracker));
    }

    public IDeferredResponse DeferredResponse { get; }

    public Task ActivityTracker { get; }
}
