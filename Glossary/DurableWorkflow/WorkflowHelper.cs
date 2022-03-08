using System;
using System.Threading.Tasks;

namespace DurableWorkflow
{
    public static class WorkflowHelper
    {
        public static async Task<TResult> RetryAsync<TResult>(Func<Task<TResult>> function, int retries, TimeSpan? delayBetweenTries = null)
        {
            Exception exception;

            do
            {
                try
                {
                    return await function.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Got exception {ex.Message}");
                    exception = ex;
                }

                await Task.Delay(delayBetweenTries ?? TimeSpan.FromSeconds(1));
            }
            while (--retries >= 0);

            throw exception;
        }
    }
}
