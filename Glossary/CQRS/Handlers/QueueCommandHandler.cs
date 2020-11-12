using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Services;

namespace CQRS.Handlers
{
    public class QueueCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandQueuingService _commandQueuingService;

        public QueueCommandHandler(ICommandQueuingService commandQueuingService)
        {
            _commandQueuingService = commandQueuingService;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _commandQueuingService.QueueAsync(command);
        }
    }
}
