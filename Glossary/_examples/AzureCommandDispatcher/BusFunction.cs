using Azure.Messaging.ServiceBus;
using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models.Commands;
using AzureCommandDispatcher.Services.Models.Responses;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace AzureCommandDispatcher;

public class BusFunction
{
    private readonly IDeferredResponseService _deferredResponseService;

    public BusFunction(
        IDeferredResponseService deferredResponseService)
    {
        _deferredResponseService = deferredResponseService;
    }

    [FunctionName("Queue")]
    public async Task RunAsync(
        [ServiceBusTrigger("multiply", Connection = "ServiceBus")] ServiceBusReceivedMessage message)
    {
        var body = message.Body;

        var deferredResponse = new DeferredResponse(message.ApplicationProperties["deferred-uri"]?.ToString() ?? throw new InvalidOperationException());

        var request = JsonConvert.DeserializeObject<MultiplyCommand>(body.ToString());

        var result = new ResultResponse { Result = request.Number1 * request.Number2 };

        await _deferredResponseService.PersistResponseAsync(deferredResponse, result);
    }
}
