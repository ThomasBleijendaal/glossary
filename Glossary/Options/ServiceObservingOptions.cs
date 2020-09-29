using System;
using Microsoft.Extensions.Options;

namespace Options
{
    public class ServiceObservingOptions : IDisposable, IMessagePoster
    {
        private readonly IOptionsMonitor<ExampleConfig> _options;
        private readonly IDisposable _optionsMonitor;

        private ExampleConfig _config;

        public ServiceObservingOptions(IOptionsMonitor<ExampleConfig> options)
        {
            _options = options;

            _optionsMonitor = _options.OnChange(config =>
            {
                _config = config;
                Console.Write("Options monitor detected change.");
            });
            _config = _options.CurrentValue;
        }

        public void PostMessage()
        {
            Console.WriteLine($"{GetType().Name}: {_config.ConfigString}");
        }

        public void Dispose()
        {
            // don't forget to dispose the event handlers, especially in Transient or Scoped services
            _optionsMonitor?.Dispose();
        }
    }
}
