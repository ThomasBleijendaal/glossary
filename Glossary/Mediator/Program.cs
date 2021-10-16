/**
* A mediator is a central object tasked to handle all direct communications to and from the components
* it mediates and manages. It allows its components to communicate with each other, without having them
* be tightly coupled.
* 
* Due to the two way communication between the components and their mediator, normal DI of often more difficult
* and more consideration should be put into it, especially when the components are stateful.
* 
* This example allows each of the components to receive external events, just like normal services. This is
* different in for example the MediatR package where external services can also use the mediator to do stuff.
* In that case, the mediator is the central entry point to every interaction, and client code does not know or
* care what object eventually handles the call.
*/

using System;
using System.Threading.Tasks;
using Mediator.Mediators;
using Mediator.Services;
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

            services.AddTransient<IUserOrderMediator, UserOrderMediator>();
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
