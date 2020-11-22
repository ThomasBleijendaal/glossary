using System;
using System.Threading.Tasks;

namespace Accessor
{
    public class SomeService
    {
        private readonly IAccessTokenAccessor _accessTokenAccessor;

        public SomeService(IAccessTokenAccessor accessTokenAccessor)
        {
            _accessTokenAccessor = accessTokenAccessor;
        }

        public async Task SomeMethodDependingOnAccessToken(string instanceName)
        {
            Console.WriteLine(instanceName);

            var token1 = _accessTokenAccessor.AccessToken;

            await Task.Delay(new Random().Next(800, 1200));

            var token2 = _accessTokenAccessor.AccessToken;

            Console.WriteLine($"{instanceName} - 1 - {token1} - {token2}");

            await Task.Delay(new Random().Next(800, 1200));

            var token3 = _accessTokenAccessor.AccessToken;

            Console.WriteLine($"{instanceName} - 2 - {token3}");
        }

        public async Task SomeMethodDependingOnAccessTokenWithConfigureAwaits(string instanceName)
        {
            Console.WriteLine(instanceName);

            var token1 = _accessTokenAccessor.AccessToken;

            await Task.Delay(new Random().Next(800, 1200)).ConfigureAwait(true);

            var token2 = _accessTokenAccessor.AccessToken;

            Console.WriteLine($"{instanceName} - 1 - {token1} - {token2}");

            await Task.Delay(new Random().Next(800, 1200)).ConfigureAwait(true);

            var token3 = _accessTokenAccessor.AccessToken;

            Console.WriteLine($"{instanceName} - 2 - {token3}");
        }
    }
}
