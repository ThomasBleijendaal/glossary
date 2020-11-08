/**
* A mediator is a central object tasked to handle all direct communications to and from the components
* it mediates and manages. It allows its components to communicate with each other, without having them
* be tightly coupled. Each component handles its own events, and then notifies the mediator about these,
* so other the other components can handle these as well.
*/

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Mediator
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, MediatorApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<IUserOderMediator, UserOrderMediator>();
        }

        public class MediatorApp : BaseApp
        {
            private readonly IUserService _userService;
            private readonly IOrderService _orderService;

            public MediatorApp(IUserService userService, IOrderService orderService)
            {
                _userService = userService;
                _orderService = orderService;
            }

            public override async Task Run()
            {
                var userId = Guid.NewGuid().ToString();

                // after user registered, some orders were made and completed correctly.
                await _orderService.CompleteOrderAsync(userId, Guid.NewGuid().ToString());
                await _orderService.CompleteOrderAsync(userId, Guid.NewGuid().ToString());
                await _orderService.CompleteOrderAsync(userId, Guid.NewGuid().ToString());
                await _orderService.CompleteOrderAsync(userId, Guid.NewGuid().ToString());

                // after some time the user wanted to delete their account
                await _userService.DeleteAsync(userId);
            }
        }
    }
}
