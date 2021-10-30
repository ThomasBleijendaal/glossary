using HttpPipeline.Policies;

namespace GatewayExample.AuthenticatedGateway;

public class AuthenticatedGatewayBearerTokenPolicy : BearerTokenPolicy
{
    private readonly string _username;
    private readonly string _password;

    public AuthenticatedGatewayBearerTokenPolicy(string username, string password)
    {
        _username = username;
        _password = password;
    }

    protected override Task<(string token, DateTimeOffset expiry)> GetBearerTokenAsync()
        => Task.FromResult(($"{_username}:{_password}", DateTimeOffset.UtcNow.AddMinutes(5)));
}
