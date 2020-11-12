using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Entities;
using CQRS.Operations;
using CQRS.Repositories;

namespace CQRS.Handlers
{
    public class DeleteBlogPostCommandHandler : ICommandHandler<DeleteBlogPostCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public DeleteBlogPostCommandHandler(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(DeleteBlogPostCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new DeleteBlogPostOperation(command));
        }
    }
}
