using AzureCommandDispatcher.Services.Abstractions;

namespace AzureCommandDispatcher.Services.Models;

internal class CommandResponse<TResponse> : ICommandResponse<TResponse>
{
    public CommandResponse(TResponse? result)
    {
        Result = result;
    }

    public CommandResponse(IDeferredResponse? deferredResponse)
    {
        DeferredResponse = deferredResponse;
    }

    public TResponse? Result { get; }

    public IDeferredResponse? DeferredResponse { get; }
}
