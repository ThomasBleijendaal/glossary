using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflowExample;

public interface IWorkflowOrchestrator<TRequest, TWorkflow, TEntity>
    where TWorkflow : IWorkflow<TRequest, TEntity>
    where TRequest : IWorkflowRequest
    where TEntity : IWorkflowEntity
{
    Task StartNewAsync(TRequest input);

    Task OrchestrateAsync(IDurableOrchestrationContext context);
}
