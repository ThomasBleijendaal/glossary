using AzureCommandDispatcher.Services.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AzureCommandDispatcher.Services.Services;

internal class DeferredResponseService : IDeferredResponseService
{
    private readonly IDatabase _database;
    private readonly ISubscriber _subscriber;

    public DeferredResponseService(
        IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public async Task PersistResponseAsync<TResponse>(IDeferredResponse deferredResponse, TResponse response)
    {
        var value = new RedisValue(JsonConvert.SerializeObject(response));

        await _database.StringSetAsync(new RedisKey(deferredResponse.Uri), value);
        await _subscriber.PublishAsync(deferredResponse.Uri, deferredResponse.Uri);
    }

    public async Task<TResponse?> ResolveResponseAsync<TResponse>(IDeferredResponse response)
        => JsonConvert.DeserializeObject<TResponse>((await _database.StringGetAsync(new RedisKey(response.Uri))) is RedisValue value && value.HasValue ? value.ToString() : "");

}
