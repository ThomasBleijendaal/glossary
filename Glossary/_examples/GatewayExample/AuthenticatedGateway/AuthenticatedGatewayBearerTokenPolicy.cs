using HttpPipeline.Messages;
using HttpPipeline.Policies;

namespace GatewayExample.AuthenticatedGateway;

public class AuthenticatedGatewayBearerTokenPolicy : BearerTokenPolicy
{
    public const string ScopePropertyKey = "scope";

    private readonly string _username;
    private readonly string _password;

    public AuthenticatedGatewayBearerTokenPolicy(string username, string password)
    {
        _username = username;
        _password = password;
    }

    protected override Task<AccessToken> GetBearerTokenAsync(string scope)
        => Task.FromResult(new AccessToken($"{scope}--{_username}:{_password}", DateTimeOffset.UtcNow.AddMinutes(5)));

    protected override string GetScope(HttpMessage message)
        => message.Request.TryGetProperty<string>(ScopePropertyKey, out var scope) 
            ? scope 
            : throw new InvalidOperationException($"Missing {ScopePropertyKey} property on request");
}
