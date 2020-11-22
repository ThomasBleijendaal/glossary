/**
 * An accessor is a resolver type object that resolves a piece of information
 * and caches it for so it can be accessed easily without repetitive cost.
 * Due to the caching the thread-safety is a point of attention.
 * 
 * The amount of work the accessor does upon accessing can be substantial 
 * and is, of course, cached.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Accessor
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, AccessorApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddSingleton<IAccessTokenAccessor, AccessTokenAccessor>();
            services.AddTransient<SomeService>();
        }

        public class AccessorApp : BaseApp
        {
            private readonly IServiceProvider _services;

            public AccessorApp(IServiceProvider services)
            {
                _services = services;
            }

            public override async Task Run()
            {
                await Task.WhenAll(
                    Task.Run(() => AsyncMethod("a")),
                    Task.Run(() => AsyncMethod("b")),
                    Task.Run(() => AsyncMethod("c")));

                await Task.Delay(5000);

                await AsyncMethodChain("configureAwait(true)");
            }

            public async Task AsyncMethod(string instanceName)
            {
                var service = _services.GetRequiredService<SomeService>();

                await service.SomeMethodDependingOnAccessToken(instanceName);
            }

            public async Task AsyncMethodChain(string instanceName)
            {
                var service = _services.GetRequiredService<SomeService>();

                await service.SomeMethodDependingOnAccessTokenWithConfigureAwaits(instanceName).ConfigureAwait(true);
            }
        }
    }
}
