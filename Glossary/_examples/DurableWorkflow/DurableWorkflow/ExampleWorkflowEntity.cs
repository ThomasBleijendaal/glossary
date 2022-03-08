using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurableWorkflow;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace DurableWorkflowExample
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExampleWorkflowEntity : IExampleWorkflowEntity
    {
        [JsonProperty]
        public List<string> Steps { get; set; } = new List<string>();

        public Task<string> Step1(string step)
        {
            Steps.Add(step);
            return Task.FromResult(step);
        }

        public Task<string> Step2(string step)
        {
            // TODO: handle this better
            return WorkflowHelper.RetryAsync(() =>
            {
                if (Random.Shared.NextDouble() < 0.5)
                {
                    throw new Exception($"BORK - {Guid.NewGuid()}");
                }

                Steps.Add(step);
                return Task.FromResult(step);
            }, 3);
        }

        public Task<string> Step3(string step)
        {
            Steps.Add(step);
            return Task.FromResult(step);
        }

        [FunctionName(nameof(ExampleWorkflowEntity))]
        public static async Task RunAsync([EntityTrigger] IDurableEntityContext ctx)
        {
            await ctx.DispatchAsync<ExampleWorkflowEntity>();
        }
    }
}
