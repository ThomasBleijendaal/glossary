using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

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

            var account = new EntityId(nameof(BankAccountEntity), accountName);
            var accountProxy = context.CreateEntityProxy<IBankAccountEntity>(account);

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

    public class TransferCommand
    {
        public string SourceAccountId { get; set; }
        public string TargetAccountId { get; set; }
        public decimal Amount { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankAccountEntity : IBankAccountEntity
    {
        [JsonProperty]
        public string AccountId { get; set; }

        [JsonProperty]
        public decimal Balance { get; set; }

        public Task<decimal> GetBalanceAsync()
        {
            return Task.FromResult(Balance);
        }

        public Task ModifyBalanceAsync(decimal delta)
        {
            Balance += delta;

            return Task.CompletedTask;
        }

        [FunctionName(nameof(BankAccountEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        {
            if (!ctx.HasState)
            {
                Console.WriteLine($"{ctx.EntityId.EntityKey}: Creating entity");

                ctx.SetState(new BankAccountEntity
                {
                    AccountId = ctx.EntityId.EntityKey,
                    Balance = 0M
                });
            }

            return ctx.DispatchAsync<BankAccountEntity>();
        }
    }

    public interface IBankAccountEntity
    {
        Task ModifyBalanceAsync(decimal delta);
        Task<decimal> GetBalanceAsync();
    }
}
