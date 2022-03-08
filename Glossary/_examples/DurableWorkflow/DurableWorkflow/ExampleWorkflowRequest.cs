using DurableWorkflow;

namespace DurableWorkflowExample
{
    public class ExampleWorkflowRequest : IWorkflowRequest
    {
        public string EntityKey => Prefix;

        public string Prefix { get; set; }
    }
}
