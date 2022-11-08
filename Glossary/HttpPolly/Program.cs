/**
 * Polly allows for creating policies to handle errors when performing tasks. One of those
 * tasks is sending Http requests.
 * 
 * To test the resilience of the systems we build with Polly we can use simmy to create chaos
 * and test how the policies handle said chaos.
 * 
 * https://www.thepollyproject.org/2019/06/27/simmy-the-monkey-for-making-chaos/
 */


using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using Polly.Extensions.Http;
using ZCommon;

public class Program : BaseProgram
{
    public static async Task Main(string[] args)
    {
        await Init<Program, PolicyApp>();
    }

    protected override void Startup(ServiceCollection services)
    {
        services.AddHttpClient<PolicyApp>()
            .AddPolicyHandler(BuildPolicy());
    }

    static IAsyncPolicy<HttpResponseMessage> BuildPolicy()
    {
        var policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                2,
                TimeSpan.FromSeconds(5),
                (x, timespan) =>
                {
                    Console.WriteLine($"Breaking for {timespan}");
                },
                () => Console.Write("Reset"));

        return policy.WrapAsync(MonkeyPolicy.InjectExceptionAsync(options =>
        {
            options.Enabled(true);
            options.InjectionRate(.5);
            options.Result(new HttpRequestException("Monkey Business", null, (HttpStatusCode)518));
        }));
    }

    public class PolicyApp : BaseApp
    {
        private readonly HttpClient _client;

        public PolicyApp(
            HttpClient client)
        {
            _client = client;
        }

        public override async Task Run()
        {
            do
            {
                try
                {
                    var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://httpbin.org/status/200"));

                    response.EnsureSuccessStatusCode();

                    Console.WriteLine("Something happend");
                }
                catch (BrokenCircuitException ex)
                {
                    Console.WriteLine($"Something bad prevented {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something bad happened {ex.Message}");
                }

                await Task.Delay(1000);
            }
            while (true);
        }
    }
}
;
