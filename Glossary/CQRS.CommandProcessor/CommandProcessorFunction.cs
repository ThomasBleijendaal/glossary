using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Handlers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CQRS.CommandProcessor
{
    public class CommandProcessorFunction
    {
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        public CommandProcessorFunction(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        [FunctionName("CommandProcessorFunction")]
        public async Task Run([QueueTrigger("command-queue", Connection = "")] string message, ILogger log)
        {
            var command = JsonConvert.DeserializeObject<ICommand>(message, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            var type = command.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(type);

            var handler = _commandHandlers.FirstOrDefault(x => handlerType.IsAssignableFrom(x.GetType()));
            if (handler == null)
            {
                throw new InvalidOperationException("This command type has no handler.");
            }

            var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));
            await (Task)method.Invoke(handler, new[] { command });
        }
    }
}
