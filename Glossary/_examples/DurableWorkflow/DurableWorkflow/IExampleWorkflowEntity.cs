using System.Threading.Tasks;
using DurableWorkflow;

namespace DurableWorkflowExample
{
    public interface IExampleWorkflowEntity : IWorkflowEntity
    {
        Task<string> Step1(string step);
        Task<string> Step2(string step);
        Task<string> Step3(string step);
    }
}
