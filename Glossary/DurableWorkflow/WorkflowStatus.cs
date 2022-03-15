using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflowExample;

public record WorkflowStatus(OrchestrationRuntimeStatus Status, OrchestrationStatus? OrchestrationStatus);
