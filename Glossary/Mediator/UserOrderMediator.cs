using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator
{
    public class UserOrderMediator : IUserOrderMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly object _initializationLock = new object();
        private bool _initialized;

        private IUserService _userService = default!;
        private IOrderService _orderService = default!;

        public UserOrderMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private void InitializeComponents()
        {
            // this is some ugly initialization to prevent circulair dependencies.
            // its better to use a library like https://github.com/jbogard/MediatR

            if (_initialized)
            {
                return;
            }

            lock (_initializationLock)
            {
                if (_initialized)
                {
                    return;
                }

                _userService = _serviceProvider.GetRequiredService<IUserService>();
                _orderService = _serviceProvider.GetRequiredService<IOrderService>();

                _initialized = true;
            }
        }

        public async Task NotifyAsync(IMediatorComponent sender, IMediationEvent @event)
        {
            InitializeComponents();

            // each of these event handling stuff is usually moved to their own event handlers 
            // in libraries like https://github.com/jbogard/MediatR/wiki
            if (@event is UserDeletionEvent userDeletionEvent)
            {
                await _orderService.DeleteOrdersAsync(userDeletionEvent.UserId);
            }
            else if (@event is OrderCompletionEvent orderCompletionEvent)
            {
                await _userService.IncreaseReputationAsync(orderCompletionEvent.UserId);
            }
        }
    }
}
