using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;

namespace DurableWorkflowExample;

public class WorkflowOrchestrator<TRequest, TWorkflow, TEntity> : IWorkflowOrchestrator<TRequest, TWorkflow, TEntity>
    where TWorkflow : IWorkflow<TRequest, TEntity>
    where TRequest : IWorkflowRequest
    where TEntity : IWorkflowEntity
{
    private readonly TWorkflow _workflow;
    private readonly IDurableClientFactory _durableClientFactory;

    public WorkflowOrchestrator(
        TWorkflow workflow,
        IDurableClientFactory durableClientFactory)
    {
        _workflow = workflow;
        _durableClientFactory = durableClientFactory;
        if (!typeof(TEntity).IsInterface)
        {
            throw new InvalidOperationException("TEntity should be an interface");
        }
    }

    public Task StartNewAsync(TRequest input)
    {
        var client = _durableClientFactory.CreateClient(new DurableClientOptions { TaskHub = "DurableTaskHub" });

        return client.StartNewAsync(typeof(TWorkflow).Name, input.InstanceId, input);
    }

    public Task OrchestrateAsync(IDurableOrchestrationContext context)
    {
        var request = context.GetInput<TRequest>();

        var entityId = new EntityId(typeof(TEntity).Name[1..], request.EntityKey);

        var proxy = context.CreateEntityProxy<TEntity>(entityId);

        return _workflow.OrchestrateAsync(context, request, entityId, proxy);
    }
}
