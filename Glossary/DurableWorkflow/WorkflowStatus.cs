using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflow
{
    public record WorkflowStatus(OrchestrationRuntimeStatus Status, OrchestrationStatus? OrchestrationStatus);
}
