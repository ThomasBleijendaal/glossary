namespace DurableWorkflowExample;

public interface IService
{
    Task DoSomethingAsync(ExampleWorkflowRequest request);
}
