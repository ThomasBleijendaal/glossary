using AzureCommandDispatcher.Services.Handlers.Base;
using AzureCommandDispatcher.Services.Models.Commands;
using AzureCommandDispatcher.Services.Models.Responses;

namespace AzureCommandDispatcher.Services.Handlers;

internal class AddCommandHandler : ImmediateCommandHandlerBase<AddCommand, ResultResponse>
{
    protected override Task<ResultResponse> HandleCommandInternalAsync(AddCommand request) => Task.FromResult(new ResultResponse { Result = request.Number1 + request.Number2 });

}
