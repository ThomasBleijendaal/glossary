using DurableWorkflowExample;

namespace DurableWorkflowExample;

public record ExampleWorkflowRequest(string Id) : IWorkflowRequest
{
    public string EntityKey => Id;

    public string InstanceId => $"{nameof(ExampleWorkflowRequest)}-{EntityKey}";
}
