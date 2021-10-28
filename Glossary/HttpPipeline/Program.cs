/**
 * TODO
 * - ErrorHandlingPolicy
 * - RetryPolicy
 * - RedirectPolicy
 * 
 * - Use BinaryData as primary buffer + test POST
 * 
 */

using HttpPipeline.GatewayExamples.AuthenticatedGateway;
using HttpPipeline.GatewayExamples.PokeGateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var collection = new ServiceCollection();

collection.AddLogging(logging => logging.AddConsole());
collection.AddHttpClient();
collection.AddSingleton<PokeGateway>();
collection.AddSingleton<AuthenticatedGateway>();

var sp = collection.BuildServiceProvider();

var logger = sp.GetRequiredService<ILogger<Program>>();

var pokeGateway = sp.GetRequiredService<PokeGateway>();

var poke = await pokeGateway.GetPokeAsync("pikachu");

Console.WriteLine(poke?.Name);

try
{
    var error = await pokeGateway.GetPokeAsync("unfindable");
}
catch (Exception ex)
{
    logger.LogError(ex, "PokeGateway error");
}



var authGateway = sp.GetRequiredService<AuthenticatedGateway>();

var auth = await authGateway.GetAuthAsync("ditto");

Console.WriteLine(auth?.Name);




Console.ReadLine();
