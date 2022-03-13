using System.Threading.Tasks;
using DurableWorkflowExample;

namespace DurableWorkflow
{
    public interface IService
    {
        Task DoSomethingAsync(ExampleWorkflowRequest request);
    }
}
