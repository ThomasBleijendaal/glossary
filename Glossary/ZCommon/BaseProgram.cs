using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ZCommon
{
    public class BaseProgram<TBaseApp>
        where TBaseApp : BaseApp
    {
        internal static async Task Main(string [] args)
        {
            var services = new ServiceCollection();

            var program = (BaseProgram<TBaseApp>)Activator.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            program.Startup(services);

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<TBaseApp>();

            await app.Run();
        }

        protected virtual void Startup(ServiceCollection services) { 
            
        }
    }

    public abstract class BaseApp
    {
        public abstract Task Run();
    }

}
