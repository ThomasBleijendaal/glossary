using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models;

namespace AzureCommandDispatcher.Services.Handlers;

internal class AsyncCommandHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    where TRequest : IRequest
{
    private readonly IAsyncDispatcher _asyncDispatcher;
    private readonly IDeferredResponseService _deferredResponseService;

    public AsyncCommandHandler(
        IAsyncDispatcher asyncDispatcher,
        IDeferredResponseService deferredResponseService)
    {
        _asyncDispatcher = asyncDispatcher;
        _deferredResponseService = deferredResponseService;
    }

    public async Task<ICommandResponse<TResponse>> HandleCommandAsync(TRequest request)
    {
        var dispatchedRequest = await _asyncDispatcher.DispatchRequestAsync(request);

        try
        {
            await dispatchedRequest.ActivityTracker.WaitAsync(TimeSpan.FromSeconds(1));
        }
        catch (TimeoutException)
        {

        }

        if (dispatchedRequest.ActivityTracker.IsCompleted)
        {
            var response = await _deferredResponseService.ResolveResponseAsync<TResponse>(dispatchedRequest.DeferredResponse);
            return new CommandResponse<TResponse>(response);
        }
        else
        {
            _asyncDispatcher.AbandonRequest(dispatchedRequest);
            return new CommandResponse<TResponse>(dispatchedRequest.DeferredResponse);
        }
    }
}
