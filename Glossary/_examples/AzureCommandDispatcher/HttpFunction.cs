using AzureCommandDispatcher.Services.Abstractions;
using AzureCommandDispatcher.Services.Models.Commands;
using AzureCommandDispatcher.Services.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureCommandDispatcher
{
    public class AddHttpFunction : HttpFunctionBase<AddCommand, ResultResponse>
    {
        public AddHttpFunction(ICommandHandler<AddCommand, ResultResponse> commandHandler) : base(commandHandler)
        {
        }

        [FunctionName("AddFunction")]
        public Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "add")] HttpRequest req) => HandleAsync(req);
    }

    public class MultiplyHttpFunction : HttpFunctionBase<MultiplyCommand, ResultResponse>
    {
        public MultiplyHttpFunction(ICommandHandler<MultiplyCommand, ResultResponse> commandHandler) : base(commandHandler)
        {
        }

        [FunctionName("MultiplyFunction")]
        public Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "multiply")] HttpRequest req) => HandleAsync(req);
    }

    public class DeferredHttpFunction
    {
        private readonly IDeferredResponseService _deferredResponseService;

        public DeferredHttpFunction(
            IDeferredResponseService deferredResponseService)
        {
            _deferredResponseService = deferredResponseService;
        }


        [FunctionName("DeferredFunction")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "deferred/{uri}")] HttpRequest req, string uri)
        {
            var result = await _deferredResponseService.ResolveResponseAsync<JObject>(new DeferredResponse(uri));

            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }
    }

    public class HttpFunctionBase<TRequest, TResponse>
        where TRequest : IRequest
    {
        private readonly ICommandHandler<TRequest, TResponse> _commandHandler;

        public HttpFunctionBase(ICommandHandler<TRequest, TResponse> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task<IActionResult> HandleAsync(HttpRequest req)
        {
            var json = await req.ReadAsStringAsync();

            var request = JsonConvert.DeserializeObject<TRequest>(json);

            if (request != null)
            {
                var result = await _commandHandler.HandleCommandAsync(request);

                if (result.Result != null)
                {
                    return new OkObjectResult(result.Result);
                }
                else if (result.DeferredResponse != null)
                {
                    return new AcceptedResult($"deferred/{result.DeferredResponse.Uri}", null);
                }
            }

            return new BadRequestResult();
        }
    }
}
