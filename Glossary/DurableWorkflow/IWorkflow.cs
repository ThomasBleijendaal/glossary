using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflow
{
    public interface IWorkflow<TRequest, TEntity>
        where TRequest : IWorkflowRequest
    {
        Task OrchestrateAsync(IDurableOrchestrationContext context, TRequest request, EntityId entityId, TEntity entity);
    }
}
