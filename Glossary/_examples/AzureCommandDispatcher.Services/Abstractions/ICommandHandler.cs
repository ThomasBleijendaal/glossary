namespace AzureCommandDispatcher.Services.Abstractions;

public interface ICommandHandler<TRequest, TResponse>
    where TRequest : IRequest
{
    Task<ICommandResponse<TResponse>> HandleCommandAsync(TRequest request);
}
