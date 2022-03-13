using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWorkflow
{
    public static class DurableOrchestrationContextExtensions
    {
        public static async Task<TResult> TryStepUntilSuccessful<TResult>(
            this IDurableOrchestrationContext context,
            Func<Task<StepResult<TResult>>> action,
            int maxRetries = 3)
        {
            var attempt = 0;
            do
            {
                var result = await action.Invoke();

                if (result.CompletedSuccessfully)
                {
                    Console.WriteLine($"Took {attempt} attempts");
                    return result.Result;
                }

                await context.CreateTimer(DateTime.UtcNow.AddSeconds(1), CancellationToken.None);
            }
            while (++attempt < maxRetries);

            throw new Exception();
        }
    }
}
