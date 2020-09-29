/**
* The options pattern allows services to demand a certain type of configuration, and prevents them from
* getting more configuration than that they requested, isolating them and making the system more secure.
* 
* If needed, a IOptionsMonitor can monitor configuration changes, like when keys rotate and services
* dependent on them need to update their settings, without restarting the whole application.
* 
* Another way of achieving dynamic options is by using a IOptionsFactory and, based on some preconditions,
* refresh the configuration manually, like after repetitive 401 errors from an external service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Options
{
    class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, ConfigApp>();
        }

        private Task _configReloadTask;

        protected override void Startup(ServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                { $"{nameof(ExampleConfig)}:{nameof(ExampleConfig.ConfigString)}", DateTime.UtcNow.ToString() }
            }).Build();

            _configReloadTask = Task.Run(async () =>
            {
                do
                {
                    // this is a bad example, but imagine that the providers (like a key vault provider) can refetch the configuration via some configuration api
                    config.Providers.First().Set($"{nameof(ExampleConfig)}:{nameof(ExampleConfig.ConfigString)}", DateTime.UtcNow.ToString());
                    config.Reload();

                    Console.WriteLine("Config reloaded!");

                    await Task.Delay(2345);
                } while (true);
            });

            services.AddOptions();
            
            services.Configure<ExampleConfig>(config.GetSection(nameof(ExampleConfig)));

            services.AddSingleton<IMessagePoster, ServiceRequiringOptions>();
            services.AddSingleton<IMessagePoster, ServiceObservingOptions>();
            services.AddSingleton<IMessagePoster, ServiceUpdatingOptions>();
        }

        public class ConfigApp : BaseApp
        {
            private readonly IEnumerable<IMessagePoster> _messagePosters;

            public ConfigApp(IEnumerable<IMessagePoster> messagePosters)
            {
                _messagePosters = messagePosters;
            }

            public override async Task Run()
            {
                do
                {
                    foreach (var poster in _messagePosters)
                    {
                        poster.PostMessage();
                    }

                    await Task.Delay(1000);
                }
                while (true);
            }
        }
    }
}
