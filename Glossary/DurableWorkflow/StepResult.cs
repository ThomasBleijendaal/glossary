namespace DurableWorkflowExample;

public record StepResult<TModel>(bool CompletedSuccessfully, TModel Result);
