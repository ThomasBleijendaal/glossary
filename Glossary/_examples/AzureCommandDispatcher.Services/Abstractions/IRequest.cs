namespace AzureCommandDispatcher.Services.Abstractions;

public interface IRequest
{
    string RequestType { get; }
}
