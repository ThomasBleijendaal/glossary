namespace AzureCommandDispatcher.Services.Abstractions;

public interface IAsyncDispatcher
{
    Task<IDispatchResponse> DispatchRequestAsync(IRequest request);
    void AbandonRequest(IDispatchResponse response);
}
