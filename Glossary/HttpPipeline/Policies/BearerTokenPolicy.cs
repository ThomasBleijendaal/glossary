using HttpPipeline.Messages;

namespace HttpPipeline.Policies;

public abstract class BearerTokenPolicy : IHttpPipelinePolicy
{
    private readonly BearerTokenCache _tokenCache;

    protected BearerTokenPolicy()
    {
        _tokenCache = new BearerTokenCache(GetBearerTokenAsync);
    }

    public async Task ProcessAsync(HttpMessage message, ReadOnlyMemory<IHttpPipelinePolicy> pipeline, NextPolicy next)
    {
        message.Request.SetHeader("Authorization", $"Bearer {await _tokenCache.GetTokenAsync()}");
        await next();
    }

    protected abstract Task<(string token, DateTimeOffset expiry)> GetBearerTokenAsync();

    private class BearerTokenCache
    {
        private readonly Func<Task<(string token, DateTimeOffset expiry)>> _tokenFactory;

        private string? _token { get; set; }
        private DateTimeOffset? _expiry { get; set; }

        public BearerTokenCache(Func<Task<(string token, DateTimeOffset expiry)>> tokenFactory)
        {
            _tokenFactory = tokenFactory;
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_token) || (_expiry.HasValue && _expiry < DateTimeOffset.UtcNow))
            {
                (_token, _expiry) = await _tokenFactory();
            }

            return _token;
        }
    }
}
