namespace DurableWorkflow
{
    public record StepResult<TModel>(bool CompletedSuccessfully, TModel Result);
}
