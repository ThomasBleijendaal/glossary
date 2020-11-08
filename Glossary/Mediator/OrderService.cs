using System;
using System.Threading.Tasks;

namespace Mediator
{
    public class OrderService : IOrderService, IMediatorComponent
    {
        private readonly IUserOderMediator _mediator;

        public OrderService(IUserOderMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CompleteOrderAsync(string userId, string orderId)
        {
            Console.WriteLine($"Order {orderId} completed.");

            await _mediator.NotifyAsync(this, new OrderCompletionEvent { UserId = userId });
        }

        public Task DeleteOrdersAsync(string userId)
        {
            Console.WriteLine($"Deleting orders from user {userId}..");

            return Task.CompletedTask;
        }
    }
}
