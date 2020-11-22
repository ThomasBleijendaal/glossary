using System;
using System.Threading.Tasks;

namespace Mediator
{
    public class UserService : IUserService, IMediatorComponent
    {
        private readonly IUserOrderMediator _mediator;

        public UserService(IUserOrderMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DeleteAsync(string userId)
        {
            Console.WriteLine($"Deleting user {userId}..");

            await _mediator.NotifyAsync(this, new UserDeletionEvent { UserId = userId });
        }

        public Task IncreaseReputationAsync(string userId)
        {
            Console.WriteLine($"Reputation of {userId} increased!");

            return Task.CompletedTask;
        }
    }
}
