namespace AzureCommandDispatcher.Services.Abstractions;

public interface IDeferredResponseService
{
    Task<TResponse?> ResolveResponseAsync<TResponse>(IDeferredResponse response);
    Task PersistResponseAsync<TResponse>(IDeferredResponse deferredResponse, TResponse response);
}
