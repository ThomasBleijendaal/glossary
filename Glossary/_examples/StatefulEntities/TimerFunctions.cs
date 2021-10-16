using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace StatefulEntities
{
    public static class TimerFunctions
    {
        [FunctionName(nameof(WageFunction))]
        public static async Task WageFunction(
            [TimerTrigger("*/1 * * * * *", RunOnStartup = true)] TimerInfo timer,
            [DurableClient] IDurableClient durableClient)
        {
            await durableClient.StartNewAsync(nameof(OrchestrationFunctions.AddWageToAccountAsync), null, "account-1");
        }

        [FunctionName(nameof(TransferFunction))]
        public static async Task TransferFunction(
            [TimerTrigger("*/10 * * * * *", RunOnStartup = true)] TimerInfo timer,
            [DurableClient] IDurableClient durableClient)
        {
            await durableClient.StartNewAsync(nameof(OrchestrationFunctions.TransferFundsAsync), new TransferCommand { Amount = 1500M, SourceAccountId = "account-1", TargetAccountId = "account-2" });
        }

        [FunctionName(nameof(OverviewFunction))]
        public static async Task OverviewFunction(
            [TimerTrigger("*/5 * * * * *", RunOnStartup = true)] TimerInfo timer,
            [DurableClient] IDurableClient durableClient)
        {
            var result = await durableClient.ListEntitiesAsync(new EntityQuery { EntityName = nameof(BankAccountEntity), FetchState = true }, CancellationToken.None);

            foreach (var entity in result.Entities)
            {
                var bankAccount = entity.State?.ToObject<BankAccountEntity>();

                if (bankAccount == null)
                {
                    Console.WriteLine($"{entity.EntityId}: Unreadable state.");
                }
                else
                {
                    Console.WriteLine($"{bankAccount.AccountId}: Balance is {bankAccount.Balance}");
                }
            }
        }
    }
}
