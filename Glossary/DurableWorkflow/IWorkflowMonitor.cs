namespace DurableWorkflow
{
    public interface IWorkflowMonitor
    {
        Task<WorkflowStatus> GetWorkflowStatusAsync(string instanceId);
    }
}
