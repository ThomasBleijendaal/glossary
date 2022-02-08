/**
 * Durable entities are a feature of Durable Functions library for Azure Functions. Key concepts:
 * 
 * - The [EntityTrigger] dispatches calls to the Durable Entities.
 * - The state of a durable entities is maintained by saving it to Azure Storage. The durable entity must have JSON serializable properties.
 * - The API surface of a durable entity should be exposed using an interface, and all available methods should be async.
 * - Via IDurableOrchestrationContext orchestration functions can access and trigger durable entities.
 *   - Durable entities are always access via a proxy which proxies interface calls to the actual durable entity.
 *   - Durable entities cannot be new-ed, they are created when a proxy calls to a new entity. It is possible to initialize the
 *      state of a durable entity when it has not state yet.
 *   - When a proxy calls a method that returns a Task, the entity is signaled in a fire-and-forget fashion.
 *   - When a proxy calls a method that returns a Task`T, the orchestration function is halted, the entity is invoked, 
 *      its result is saved, and the orchestration function is replayed up to the proxy call and the result from that invocation is used.
 *   - It is possible to lock durable entities when some critical functionality requires exclusive access to these entities.
 *     - Only locked entities can be access in the critical section.
 *     - Other proxy calls to locked entities are automatically queued and executed after the lock has been removed.
 */

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
