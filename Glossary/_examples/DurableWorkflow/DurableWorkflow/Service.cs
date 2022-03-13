using System.Threading.Tasks;
using DurableWorkflowExample;

namespace DurableWorkflow
{
    public class Service : IService
    {
        private readonly IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> _workflowOrchestrator;

        public Service(
            IWorkflowOrchestrator<ExampleWorkflowRequest, ExampleWorkflow, IExampleWorkflowEntity> workflowOrchestrator)
        {
            _workflowOrchestrator = workflowOrchestrator;
        }

        public Task DoSomethingAsync(ExampleWorkflowRequest request)
        {
            return _workflowOrchestrator.StartNewAsync(request);
        }
    }
}
