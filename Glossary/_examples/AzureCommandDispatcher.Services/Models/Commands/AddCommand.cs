using AzureCommandDispatcher.Services.Abstractions;

namespace AzureCommandDispatcher.Services.Models.Commands;

public class AddCommand : IRequest
{
    public string RequestType => "add";

    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
