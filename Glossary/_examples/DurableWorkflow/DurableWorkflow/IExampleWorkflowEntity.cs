using System.Threading.Tasks;
using DurableWorkflow;

namespace DurableWorkflowExample
{
    public interface IExampleWorkflowEntity : IWorkflowEntity
    {
        Task<StepResult<string>> Step1(string step);
        Task<StepResult<string>> Step2(string step);
        Task<StepResult<int>> Step3(string step);
    }
}
