/**
 * A command dispatcher is pattern that a centralized dispatcher dispatches commands to 
 * command handlers based on what the command is. 
 * This prevents the caller of the dispatcher to know which handlers should be invoked,
 * and also prevents the api surface of the dispatcher to become huge with all the possible
 * commands and results.
 * 
 * In this example the command handlers are bound to a type of dispatcher, to prevent that all
 * command handlers are resolved by the DI to all dispatchers.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace CommandDispatcher
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, CommandDispatcherApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddTransient<ICommandHandler<IExampleCommandDispatcher>, CommandA1Handler>();
            services.AddTransient<ICommandHandler<IExampleCommandDispatcher>, CommandA2Handler>();
            services.AddTransient<ICommandHandler<IExampleCommandDispatcher>, CommandB1Handler>();
            services.AddTransient<ICommandHandler<IExampleCommandDispatcher>, CommandB2Handler>();

            services.AddTransient<IExampleCommandDispatcher, ExampleCommandDispatcher>();
        }

        public class CommandDispatcherApp : BaseApp
        {
            private readonly IExampleCommandDispatcher _exampleCommandDispatcher;

            public CommandDispatcherApp(IExampleCommandDispatcher exampleCommandDispatcher)
            {
                _exampleCommandDispatcher = exampleCommandDispatcher;
            }

            public override async Task Run()
            {
                var commandA = new CommandA();
                var commandB = new CommandB();

                var resulta1 = await _exampleCommandDispatcher.Dispatch<CommandA, Result1>(commandA);
                var resulta2 = await _exampleCommandDispatcher.Dispatch<CommandA, Result2>(commandA);
                var resultb1 = await _exampleCommandDispatcher.Dispatch<CommandB, Result1>(commandB);
                var resultb2 = await _exampleCommandDispatcher.Dispatch<CommandB, Result2>(commandB);

                Console.WriteLine(resulta1.GetType());
                Console.WriteLine(resulta2.GetType());
                Console.WriteLine(resultb1.GetType());
                Console.WriteLine(resultb2.GetType());

                Console.ReadLine();
            }
        }
    }
}
