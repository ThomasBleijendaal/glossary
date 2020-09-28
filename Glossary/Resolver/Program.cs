/**
 * A resolver is a one trick pony that resolves a piece of information that can be reused
 * everwhere it's needed. A resolver fetches the data everytime its requested and will not
 * cache it.
 * 
 * A resolver should remain simple.
 */
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Resolver
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, ResolverApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddSingleton<IAccessTokenResolver, AccessTokenResolver>();
            services.AddTransient<ISomeService, SomeService>();
        }

        public class ResolverApp : BaseApp
        {
            private readonly ISomeService _someService;

            public ResolverApp(ISomeService someService)
            {
                _someService = someService;
            }

            public override Task Run()
            {
                _someService.SomeMethodRequiringAccessToken();

                return Task.CompletedTask;
            }
        }
    }
}
