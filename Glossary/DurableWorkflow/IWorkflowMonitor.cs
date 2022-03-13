namespace DurableWorkflowExample;

public interface IWorkflowMonitor
{
    Task<WorkflowStatus> GetWorkflowStatusAsync(string instanceId);
}
