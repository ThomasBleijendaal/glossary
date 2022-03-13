using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableWorkflowExample;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace DurableWorkflow
{
    public class Function
    {
        private readonly IService _service;
        private readonly IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> _workflowOrchestrator;
        private readonly IWorkflowMonitor _workflowMonitor;

        private static Guid _lastId;

        public Function(
            IService service,
            IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> workflowOrchestrator,
            IWorkflowMonitor workflowMonitor)
        {
            _service = service;
            _workflowOrchestrator = workflowOrchestrator;
            _workflowMonitor = workflowMonitor;
        }

        [FunctionName("Function1_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter)
        {
            await starter.PurgeInstanceHistoryAsync(null);

            _lastId = Guid.NewGuid();

            var request = new ExampleWorkflowRequest(_lastId.ToString());

            await _service.DoSomethingAsync(request);

            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        [FunctionName("Function1_Status")]
        public async Task<HttpResponseMessage> Status(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req)
        {
            var request = new ExampleWorkflowRequest(_lastId.ToString());

            var status = await _workflowMonitor.GetWorkflowStatusAsync(request.InstanceId);

            var response = status.Status switch
            {
                OrchestrationRuntimeStatus.Running => req.CreateResponse(HttpStatusCode.Accepted),
                OrchestrationRuntimeStatus.Completed => req.CreateResponse(HttpStatusCode.OK),
                OrchestrationRuntimeStatus.ContinuedAsNew => req.CreateResponse(HttpStatusCode.Accepted),
                OrchestrationRuntimeStatus.Failed => req.CreateResponse(HttpStatusCode.InternalServerError),
                OrchestrationRuntimeStatus.Canceled => req.CreateResponse(HttpStatusCode.InternalServerError),
                OrchestrationRuntimeStatus.Terminated => req.CreateResponse(HttpStatusCode.InternalServerError),
                OrchestrationRuntimeStatus.Pending => req.CreateResponse(HttpStatusCode.Accepted),
                _ => req.CreateResponse(HttpStatusCode.NotFound)
            };

            response.Content = new StringContent(JsonConvert.SerializeObject(status.OrchestrationStatus));

            return response;
        }

        [FunctionName(nameof(ExampleWorkflow))]
        public async Task OrchestrateAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await _workflowOrchestrator.OrchestrateAsync(context);
        }
    }
}
