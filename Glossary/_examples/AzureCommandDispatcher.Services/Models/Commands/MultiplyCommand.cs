using AzureCommandDispatcher.Services.Abstractions;

namespace AzureCommandDispatcher.Services.Models.Commands;

public class MultiplyCommand : IRequest
{
    public string RequestType => "multiply";

    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
