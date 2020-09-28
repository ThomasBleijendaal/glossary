/**
 * A provider is a class that provides a certain strategy to allow for selecting a
 * certain algorithm at runtime. The context using the provider will just use it as
 * an interface and accept its result.
 * 
 * A provider should not be substantial and should remain simple.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Provider
{

    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, ProviderApp>();
            await Init<Program, ProviderApp>();
            await Init<Program, ProviderApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            // this could be somewhere in a service, that based on some logic a different provider will be selected and used in the relevant context
            if (new Random().Next(0, 9) < 5)
            {
                services.AddTransient<ILogMessageProvider, MessageAProvider>();
            }
            else
            {
                services.AddTransient<ILogMessageProvider, MessageBProvider>();
            }
        }

        public class ProviderApp : BaseApp
        {
            private readonly ILogMessageProvider _logMessageProvider;

            public ProviderApp(ILogMessageProvider logMessageProvider)
            {
                _logMessageProvider = logMessageProvider;
            }

            public override Task Run()
            {
                Console.WriteLine(_logMessageProvider.CreateMessage("World"));

                return Task.CompletedTask;
            }
        }
    }
}
