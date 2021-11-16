using System;
using GatewayExample.AuthenticatedGateway;
using GatewayExample.PokeGateway;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var collection = new ServiceCollection();

collection.AddLogging(logging => logging.AddConsole());
collection.AddHttpClient();
collection.AddMemoryCache();
collection.AddSingleton<PokeGateway>();
collection.AddSingleton<AuthenticatedGateway>();

var sp = collection.BuildServiceProvider();

var logger = sp.GetRequiredService<ILogger<Program>>();

var pokeGateway = sp.GetRequiredService<PokeGateway>();

var poke = await pokeGateway.GetPokeAsync("pikachu");

// subsequent requests within the cache timeout of the http pipeline will return cached response
var poke1 = await pokeGateway.GetPokeAsync("pikachu");
var poke2 = await pokeGateway.GetPokeAsync("pikachu");
var poke3 = await pokeGateway.GetPokeAsync("pikachu");

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

var auth1 = await authGateway.GetAuthAsync("ditto");
Console.WriteLine(auth1?.Name);

var auth2 = await authGateway.GetAuthAsync("ditto");
Console.WriteLine(auth2?.Name);

// -- 

Console.ReadLine();
