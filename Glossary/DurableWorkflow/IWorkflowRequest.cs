namespace DurableWorkflowExample;

public interface IWorkflowRequest
{
    public string EntityKey { get; }

    public string InstanceId { get; }
}
