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

            await Task.Delay(new Random().Next(800, 1200));

            var token = _accessTokenAccessor.AccessToken;

            Console.WriteLine($"{instanceName} - 1 - {token}");

            await Task.Delay(new Random().Next(800, 1200));

            Console.WriteLine($"{instanceName} - 2 - {token}");
        }
    }
}
