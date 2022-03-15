using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;

namespace DurableWorkflowExample;

public class WorkflowMonitor : IWorkflowMonitor
{
    private readonly IDurableClientFactory _durableClientFactory;

    public WorkflowMonitor(
        IDurableClientFactory durableClientFactory)
    {
        _durableClientFactory = durableClientFactory;
    }

    public async Task<WorkflowStatus> GetWorkflowStatusAsync(string id)
    {
        var client = _durableClientFactory.CreateClient(new DurableClientOptions { TaskHub = "DurableTaskHub" });

        var status = await client.GetStatusAsync(id);

        return new WorkflowStatus(
            status?.RuntimeStatus ?? OrchestrationRuntimeStatus.Unknown,
            status?.CustomStatus?.ToObject<OrchestrationStatus>());
    }
}
