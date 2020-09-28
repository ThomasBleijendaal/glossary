using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ZCommon
{
    public class BaseProgram
    {
        public static async Task Init<TInit, TApp>()
            where TApp : BaseApp
        {
            var services = new ServiceCollection();

            var program = (BaseProgram)Activator.CreateInstance(typeof(TInit));

            services.AddTransient<TApp>();

            program.Startup(services);

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var app = scope.ServiceProvider.GetRequiredService<TApp>();

            await app.Run();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        protected virtual void Startup(ServiceCollection services)
        {

        }
    }
}
