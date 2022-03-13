using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DurableWorkflow;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableWorkflowExample
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExampleWorkflowEntity : IExampleWorkflowEntity
    {
        private readonly ILogger<ExampleWorkflowEntity> _logger;

        public ExampleWorkflowEntity(
            ILogger<ExampleWorkflowEntity> logger)
        {
            _logger = logger;
        }

        [JsonProperty]
        public List<string> Steps { get; set; } = new List<string>();

        public async Task<StepResult<string>> Step1(string step)
        {
            await Task.Delay(2000);

            Steps.Add(step);
            return new StepResult<string>(true, step);
        }

        public async Task<StepResult<string>> Step2(string step)
        {
            await Task.Delay(2000);

            if (Random.Shared.NextDouble() < 0.5)
            {
                return new StepResult<string>(false, string.Empty);
            }

            Steps.Add(step);
            return new StepResult<string>(true, step);
        }

        public async Task<StepResult<int>> Step3(string step)
        {
            await Task.Delay(2000);

            Steps.Add(step);
            return new StepResult<int>(true, Steps.Count);
        }

        [FunctionName(nameof(ExampleWorkflowEntity))]
        public static Task RunAsync(
            [EntityTrigger] IDurableEntityContext ctx,
            ILogger logger)
            => ctx.DispatchAsync<ExampleWorkflowEntity>();
    }
}
