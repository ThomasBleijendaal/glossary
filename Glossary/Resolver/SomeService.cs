using System;

namespace Resolver
{
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

            Console.WriteLine(token);

            // do stuff with it.
        }
    }
}
