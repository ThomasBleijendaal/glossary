using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableWorkflowExample;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableWorkflow
{
    public class Function
    {
        private readonly IService _service;
        private readonly IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> _workflowOrchestrator;

        public Function(
            IService service,
            IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> workflowOrchestrator)
        {
            _service = service;
            _workflowOrchestrator = workflowOrchestrator;
        }

        [FunctionName("Function1_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            await starter.PurgeInstanceHistoryAsync(null);

            await _service.DoSomethingAsync();

            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        [FunctionName(nameof(ExampleWorkflow))]
        public async Task OrchestrateAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await _workflowOrchestrator.OrchestrateAsync(context);
        }
    }

    public interface IService
    {
        Task DoSomethingAsync();
    }

    public class Service : IService
    {
        private readonly IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> _workflowOrchestrator;

        public Service(
            IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> workflowOrchestrator)
        {
            _workflowOrchestrator = workflowOrchestrator;
        }

        public Task DoSomethingAsync()
        {
            return _workflowOrchestrator.StartNewAsync(new ExampleWorkflowRequest { Prefix = Guid.NewGuid().ToString() });
        }
    }
}
