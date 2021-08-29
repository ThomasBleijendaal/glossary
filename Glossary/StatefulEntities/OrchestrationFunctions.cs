using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace StatefulEntities
{
    public static class OrchestrationFunctions
    {
        [FunctionName(nameof(AddWageToAccountAsync))]
        public static async Task AddWageToAccountAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var accountName = context.GetInput<string>();
            if (string.IsNullOrEmpty(accountName))
            {
                return;
            }

            var accountProxy = context.CreateEntityProxy<IBankAccountEntity>(accountName);

            Console.WriteLine($"{accountName}: Adding wage");
            // although this does not lock the account, the proxy will still queue this command when the account happened to be locked
            await accountProxy.ModifyBalanceAsync(100);
            Console.WriteLine($"{accountName}: Added wage");
        }

        [FunctionName(nameof(TransferFundsAsync))]
        public static async Task TransferFundsAsync(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var transferCommand = context.GetInput<TransferCommand>();
            if (transferCommand == null)
            {
                return;
            }

            var source = new EntityId(nameof(BankAccountEntity), transferCommand.SourceAccountId);
            var target = new EntityId(nameof(BankAccountEntity), transferCommand.TargetAccountId);

            using (await context.LockAsync(source, target))
            {
                Console.WriteLine($"{transferCommand.SourceAccountId}: Locked");
                Console.WriteLine($"{transferCommand.TargetAccountId}: Locked");

                var sourceProxy = context.CreateEntityProxy<IBankAccountEntity>(source);
                if (await sourceProxy.GetBalanceAsync() < transferCommand.Amount)
                {
                    Console.WriteLine($"{transferCommand.SourceAccountId}: Is broke. Bouncing transfer.");
                    return;
                }

                var targetProxy = context.CreateEntityProxy<IBankAccountEntity>(target);

                await sourceProxy.ModifyBalanceAsync(-transferCommand.Amount);
                Console.WriteLine($"{transferCommand.SourceAccountId}: Taken amount out");
                await targetProxy.ModifyBalanceAsync(transferCommand.Amount);
                Console.WriteLine($"{transferCommand.TargetAccountId}: Added amount to");
            }

            Console.WriteLine($"{transferCommand.SourceAccountId}: Unlocked");
            Console.WriteLine($"{transferCommand.TargetAccountId}: Unlocked");
        }
    }
}
