using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Services;

namespace CQRS.Handlers
{
    public class QueueCommandHandler :
        ICommandHandler<AddLikeCommand>,
        ICommandHandler<AddCommentCommand>,
        ICommandHandler<DeleteBlogPostCommand>
    {
        private readonly ICommandQueuingService _commandQueuingService;

        public QueueCommandHandler(ICommandQueuingService commandQueuingService)
        {
            _commandQueuingService = commandQueuingService;
        }

        public async Task HandleAsync(AddLikeCommand command)
        {
            await _commandQueuingService.QueueAsync(command);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            await _commandQueuingService.QueueAsync(command);
        }

        public async Task HandleAsync(DeleteBlogPostCommand command)
        {
            await _commandQueuingService.QueueAsync(command);
        }
    }
}
