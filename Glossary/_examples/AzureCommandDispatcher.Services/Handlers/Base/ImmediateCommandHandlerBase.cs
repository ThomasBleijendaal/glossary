using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models;

namespace AzureCommandDispatcher.Services.Handlers.Base;

internal abstract class ImmediateCommandHandlerBase<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    where TRequest : IRequest
{
    public async Task<ICommandResponse<TResponse>> HandleCommandAsync(TRequest request)
        => new CommandResponse<TResponse>(await HandleCommandInternalAsync(request));

    protected abstract Task<TResponse> HandleCommandInternalAsync(TRequest request);
}
