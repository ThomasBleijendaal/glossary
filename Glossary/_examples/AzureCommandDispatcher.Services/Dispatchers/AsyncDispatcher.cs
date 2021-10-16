using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models.Responses;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AzureCommandDispatcher.Services.Dispatchers;

internal class AsyncDispatcher : IAsyncDispatcher, IDisposable
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ConcurrentDictionary<string, TaskCompletionSource> _valueSubscriptions;

    public AsyncDispatcher(
        IConnectionMultiplexer connectionMultiplexer,
        ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;

        var subscriber = connectionMultiplexer.GetSubscriber();

        subscriber
            .Subscribe(new RedisChannel("async-*", RedisChannel.PatternMode.Auto))
            .OnMessage(HandleUpdateMessage);

        _valueSubscriptions = new ConcurrentDictionary<string, TaskCompletionSource>();
    }

    public async Task<IDispatchResponse> DispatchRequestAsync(IRequest request)
    {
        var deferredResponse = new DeferredResponse($"async-{request.RequestType}-{Guid.NewGuid()}");

        var tcs = new TaskCompletionSource();

        _valueSubscriptions.TryAdd(deferredResponse.Uri, tcs);

        var dispatchResponse = new DispatchResponse(deferredResponse, tcs.Task);

        var sender = _serviceBusClient.CreateSender(request.RequestType);

        var message = new ServiceBusMessage(JsonConvert.SerializeObject(request));
        message.ApplicationProperties.Add("deferred-uri", deferredResponse.Uri);

        await sender.SendMessageAsync(message);


        return dispatchResponse;
    }

    public void AbandonRequest(IDispatchResponse response)
    {
        _valueSubscriptions.TryRemove(response.DeferredResponse.Uri, out var _);
    }

    private void HandleUpdateMessage(ChannelMessage message)
    {
        if (_valueSubscriptions.TryGetValue(message.Channel.ToString(), out var tcs))
        {
            tcs.SetResult();
        }
    }

    public void Dispose()
    {
        foreach (var element in _valueSubscriptions)
        {
            element.Value.TrySetCanceled();
        }
    }
}
