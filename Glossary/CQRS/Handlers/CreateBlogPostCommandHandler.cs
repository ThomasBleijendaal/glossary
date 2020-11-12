using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Creations;
using CQRS.Entities;
using CQRS.Repositories;

namespace CQRS.Handlers
{
    public class CreateBlogPostCommandHandler : ICommandHandler<CreateBlogPostCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public CreateBlogPostCommandHandler(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(CreateBlogPostCommand command)
        {
            await _repository.CreateEntityAsync(new BlogPostCreation(command));
        }

    }
}
