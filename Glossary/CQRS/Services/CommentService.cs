using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Handlers;

namespace CQRS.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommandHandler<AddCommentCommand> _addCommentHandler;
        private readonly ICommandHandler<AddLikeCommand> _addLikeHandler;

        public CommentService(
            ICommandHandler<AddCommentCommand> addCommentHandler,
            ICommandHandler<AddLikeCommand> addLikeHandler)
        {
            _addCommentHandler = addCommentHandler;
            _addLikeHandler = addLikeHandler;
        }

        public async Task AddCommentAsync(int blogPostId, string author, string comment)
        {
            await _addCommentHandler.HandleAsync(new AddCommentCommand
            {
                Author = author,
                BlogPostId = blogPostId,
                Comment = comment
            });
        }

        public async Task AddLikeAsync(int blogPostId)
        {
            await _addLikeHandler.HandleAsync(new AddLikeCommand
            {
                BlogPostId = blogPostId
            });
        }
    }
}
