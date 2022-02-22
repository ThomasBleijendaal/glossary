/**
* The options pattern allows services to demand a certain type of configuration, and prevents them from
* getting more configuration than that they requested, isolating them and making the system more secure.
* 
* If needed, a IOptionsMonitor can monitor configuration changes, like when keys rotate and services
* dependent on them need to update their settings, without restarting the whole application.
* 
* Another way of achieving dynamic options is by using a IOptionsFactory and, based on some preconditions,
* refresh the configuration manually, like after a 401 error from an external service.
* 
* IOptionsSnapshot can be used in conjunction with options providers which regularly update,
* like KeyVault which fetches new keys and secrets. The IOptionsSnapshot is a scoped service which only
* caches its Value for its lifetime, which is, as it is scoped, limited.
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
                    RefreshConfig(config);
                    config.Reload();

                    Console.WriteLine("Config reloaded!");

                    await Task.Delay(2345);
                } while (true);
            });

            services.AddOptions();

            services.Configure<ExampleConfig>(config.GetSection(nameof(ExampleConfig)));

            services.AddSingleton<IMessagePoster, ServiceRequiringOptions>();
            services.AddSingleton<IMessagePoster, ServiceObservingOptions>();
            services.AddScoped<IMessagePoster, ServiceSnapshottingOptions>();
            services.AddSingleton<IMessagePoster, ServiceUpdatingOptions>();
        }

        public class ConfigApp : BaseApp
        {
            private readonly IServiceProvider _serviceProvider;

            public ConfigApp(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public override async Task Run()
            {
                do
                {
                    using var scope = _serviceProvider.CreateScope();

                    foreach (var poster in scope.ServiceProvider.GetRequiredService<IEnumerable<IMessagePoster>>())
                    {
                        poster.PostMessage();
                    }

                    await Task.Delay(1000);
                }
                while (true);
            }
        }

        private static void RefreshConfig(IConfigurationRoot config)
        {
            // this is a bad example, but imagine that the providers (like a key vault provider) can refetch the configuration via some configuration api
            var memoryCollection = config.Providers.First();
            memoryCollection.Set($"{nameof(ExampleConfig)}:{nameof(ExampleConfig.ConfigString)}", DateTime.UtcNow.ToString());
        }
    }
}
