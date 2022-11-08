using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

public abstract class BearerTokenPolicy : IHttpPipelinePolicy
{
    private readonly BearerTokenCache _tokenCache;

    protected BearerTokenPolicy()
    {
        _tokenCache = new BearerTokenCache(GetBearerTokenAsync);
    }

    public async Task ProcessAsync(HttpMessage message, NextPolicy next)
    {
        message.Request.SetHeader("Authorization", $"Bearer {await _tokenCache.GetTokenAsync(GetScope(message))}");
        await next();
    }

    protected abstract string GetScope(HttpMessage message);
    protected abstract Task<AccessToken> GetBearerTokenAsync(string scope);

    private class BearerTokenCache
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly Func<string, Task<AccessToken>> _tokenFactory;

        private readonly Dictionary<string, AccessToken> _tokenCache = new Dictionary<string, AccessToken>();

        public BearerTokenCache(Func<string, Task<AccessToken>> tokenFactory)
        {
            _tokenFactory = tokenFactory;
        }

        public async Task<string> GetTokenAsync(string scope)
        {
            if (GetValidToken(scope, out var cachedToken))
            {
                return cachedToken.Token;
            }

            try
            {
                await _semaphore.WaitAsync();

                if (GetValidToken(scope, out var recentlyCachedToken))
                {
                    return recentlyCachedToken.Token;
                }

                var newToken = await _tokenFactory(scope);

                _tokenCache[scope] = newToken;

                return newToken.Token;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private bool GetValidToken(string scope, out AccessToken token)
        {
            if (_tokenCache.TryGetValue(scope, out var accessToken) && accessToken.Expiry > DateTimeOffset.UtcNow)
            {
                token = accessToken;
                return true;
            }
            else
            {
                token = new AccessToken("", DateTimeOffset.MinValue);
                return false;
            }
        }
    }

    public class AccessToken
    {
        public AccessToken(string token, DateTimeOffset expiry)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            Expiry = expiry;
        }

        public string Token { get; set; }
        public DateTimeOffset Expiry { get; set; }
    }
}
