namespace AzureCommandDispatcher.Services.Abstractions;

public interface ICommandResponse<TResponse>
{
    TResponse? Result { get; }
    IDeferredResponse? DeferredResponse { get; }
}
