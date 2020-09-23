/**
 * A resolver is a one trick pony that resolves a piece of information that can be reused
 * everwhere it's needed. A resolver fetches the data everytime its requested and will not
 * cache it.
 */
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Resolver
{
    public interface IAccessTokenResolver
    {
        string AccessToken { get; }
    }

    public class AccessTokenResolver : IAccessTokenResolver
    {
        // usually implemented as _httpContextAccessor.HttpContext.Headers["Authorization"].Replace("Bearer ", "");
        public string AccessToken => "some token";
    }

    public interface ISomeService
    {
        void SomeMethodRequiringAccessToken();
    }

    public class SomeService : ISomeService
    {
        private readonly IAccessTokenResolver _accessTokenResolver;

        public SomeService(IAccessTokenResolver accessTokenResolver)
        {
            _accessTokenResolver = accessTokenResolver;
        }

        public void SomeMethodRequiringAccessToken()
        {
            var token = _accessTokenResolver.AccessToken;

            Console.Write(token);

            // do stuff with it.
        }
    }

    public class Program : BaseProgram<Program.ResolverApp>
    {
        private static IAccessTokenResolver _accessTokenResolver = new AccessTokenResolver();

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

            }
        }
    }
}
