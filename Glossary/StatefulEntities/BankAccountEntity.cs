using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace StatefulEntities
{
    public class BankAccountEntity : BankAccountState, IBankAccountEntity
    {
        private readonly ILogger<BankAccountEntity> _logger;

        public BankAccountEntity(ILogger<BankAccountEntity> logger)
        {
            _logger = logger;
        }

        public Task<decimal> GetBalanceAsync()
        {
            _logger.LogInformation("{accountId}: Getting balance", AccountId);
            return Task.FromResult(Balance);
        }

        public Task ModifyBalanceAsync(decimal delta)
        {
            _logger.LogInformation("{accountId}: Applying {delta}", AccountId, delta);
            Balance += delta;

            return Task.CompletedTask;
        }

        [FunctionName(nameof(BankAccountEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        {
            if (!ctx.HasState)
            {
                Console.WriteLine($"{ctx.EntityId.EntityKey}: Creating entity");

                ctx.SetState(new BankAccountState
                {
                    AccountId = ctx.EntityId.EntityKey,
                    Balance = 0M
                });
            }

            return ctx.DispatchAsync<BankAccountEntity>();
        }
    }
}
