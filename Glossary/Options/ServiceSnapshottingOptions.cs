using System;
using Microsoft.Extensions.Options;

namespace Options
{
    public class ServiceSnapshottingOptions : IMessagePoster
    {
        private readonly ExampleConfig _config;

        public ServiceSnapshottingOptions(IOptionsSnapshot<ExampleConfig> options)
        {
            _config = options.Value;
        }

        public void PostMessage()
        {
            Console.WriteLine($"{GetType().Name}: {_config.ConfigString}");
        }
    }
}
