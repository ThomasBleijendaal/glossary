namespace AzureCommandDispatcher.Services.Abstractions;

public interface IDispatchResponse
{
    IDeferredResponse DeferredResponse { get; }
    Task ActivityTracker { get; }
}
