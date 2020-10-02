using System;
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
        private readonly ICommandHandler<AddCommentCommand> _addCommentHandler;
        private readonly ICommandHandler<AddLikeCommand> _addLikeHandler;

        public CommandProcessorFunction(
            ICommandHandler<AddCommentCommand> addCommentHandler,
            ICommandHandler<AddLikeCommand> addLikeHandler)
        {
            _addCommentHandler = addCommentHandler;
            _addLikeHandler = addLikeHandler;
        }

        [FunctionName("CommandProcessorFunction")]
        public async Task Run([QueueTrigger("command-queue", Connection = "")]string message, ILogger log)
        {
            var command = JsonConvert.DeserializeObject<ICommand>(message, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            switch (command)
            {
                case AddCommentCommand addComment:
                    await _addCommentHandler.HandleAsync(addComment);
                    break;

                case AddLikeCommand addLike:
                    await _addLikeHandler.HandleAsync(addLike);
                    break;

                default:
                    throw new InvalidOperationException("This command type should not be queued.");
            }
        }
    }
}
