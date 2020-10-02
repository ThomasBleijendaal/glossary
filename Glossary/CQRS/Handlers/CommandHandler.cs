using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Creations;
using CQRS.Entities;
using CQRS.Operations;
using CQRS.Repositories;

namespace CQRS.Handlers
{
    public class CommandHandler :
        ICommandHandler<CreateBlogPostCommand>,
        ICommandHandler<DeleteBlogPostCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public CommandHandler(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(CreateBlogPostCommand command)
        {
            await _repository.CreateEntityAsync(new BlogPostCreation(command));
        }

        public async Task HandleAsync(DeleteBlogPostCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new DeleteBlogPostOperation(command));
        }
    }
}
