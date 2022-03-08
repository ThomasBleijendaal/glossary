using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableWorkflow;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflowExample
{
    public class ExampleWorkflow : IWorkflow<ExampleWorkflowRequest, IExampleWorkflowEntity>
    {
        public async Task OrchestrateAsync(IDurableOrchestrationContext context, ExampleWorkflowRequest init, EntityId entityId, IExampleWorkflowEntity entity)
        {
            var step1Result = await entity.Step1("1");

            var step2Results = new List<string>();

            using (await context.LockAsync(entityId))
            {
                foreach (var i in Enumerable.Range(0, 3))
                {
                    var step2Result = await entity.Step2($"2-{step1Result}-{i}");

                    step2Results.Add(step2Result);
                }
            }

            var finalResult = await entity.Step3($"3-{string.Join(",", step2Results)}");

            Console.WriteLine(finalResult);
        }
    }
}
