using System;
using Microsoft.Extensions.Options;

namespace Options
{
    public class ServiceUpdatingOptions : IMessagePoster
    {
        private readonly IOptionsFactory<ExampleConfig> _optionsFactory;

        private ExampleConfig _config;

        public ServiceUpdatingOptions(IOptionsFactory<ExampleConfig> optionsFactory)
        {
            _optionsFactory = optionsFactory;

            UpdateConfig();
        }

        private void UpdateConfig()
        {
            // the default anonymous option has an empty string as name
            _config = _optionsFactory.Create(string.Empty);
        }

        public void PostMessage()
        {
            Console.WriteLine($"{GetType().Name}: {_config.ConfigString}");

            // imagine that check is like a check on response codes coming from an external service and after detection of a 401
            // it tries to get the new api key from key vault by refreshing its configuration
            if (new Random().Next(0, 9) > 6)
            {
                Console.WriteLine("Detected outdated configuration. Refreshing..");

                UpdateConfig();
            }
        }
    }
}
