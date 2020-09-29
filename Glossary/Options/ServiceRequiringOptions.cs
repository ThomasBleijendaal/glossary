using System;
using Microsoft.Extensions.Options;

namespace Options
{
    public class ServiceRequiringOptions : IMessagePoster
    {
        private readonly ExampleConfig _config;

        public ServiceRequiringOptions(IOptions<ExampleConfig> options)
        {
            _config = options.Value;
        }

        public void PostMessage()
        {
            Console.WriteLine($"{GetType().Name}: {_config.ConfigString}");
        }
    }
}
