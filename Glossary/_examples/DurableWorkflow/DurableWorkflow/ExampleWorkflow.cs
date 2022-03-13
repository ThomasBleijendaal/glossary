using DurableWorkflowExample;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableWorkflowExample;

public class ExampleWorkflow : IWorkflow<ExampleWorkflowRequest, IExampleWorkflowEntity>
{
    private readonly ILogger<ExampleWorkflow> _logger;

    public ExampleWorkflow(
        ILogger<ExampleWorkflow> logger)
    {
        _logger = logger;
    }

    public async Task OrchestrateAsync(IDurableOrchestrationContext context, ExampleWorkflowRequest init, EntityId entityId, IExampleWorkflowEntity entity)
    {
        var logger = context.CreateReplaySafeLogger(_logger);

        context.SetCustomStatus(new OrchestrationStatus(1, 3));

        var step1Result = await entity.Step1("1");

        context.SetCustomStatus(new OrchestrationStatus(2, 3));

        var step2Results = new List<string>();

        foreach (var i in Enumerable.Range(0, 3))
        {
            var step2Result = await context.TryStepUntilSuccessful(() => entity.Step2($"2-{step1Result.Result}-{i}"));

            step2Results.Add(step2Result);
        }

        context.SetCustomStatus(new OrchestrationStatus(3, 3));

        using (await context.LockAsync(entityId))
        {
            await entity.Step3($"3-{string.Join(",", step2Results)}");
        }

        logger.LogInformation("DONE");
    }
}
