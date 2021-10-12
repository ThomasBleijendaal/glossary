using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models;
using AzureCommandDispatcher.Services.Models.Commands;
using AzureCommandDispatcher.Services.Models.Responses;

namespace AzureCommandDispatcher.Services.Handlers;

internal class MultiplyCommandHandler : ICommandHandler<MultiplyCommand, ResultResponse>
{
    public Task<ICommandResponse<ResultResponse>> HandleCommandAsync(MultiplyCommand request)
    {
        return Task.FromResult<ICommandResponse<ResultResponse>>(new CommandResponse<ResultResponse>(new ResultResponse { Result = request.Number1 * request.Number2 }));
    }
}
