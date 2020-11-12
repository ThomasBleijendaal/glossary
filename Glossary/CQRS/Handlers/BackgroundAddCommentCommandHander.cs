using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Entities;
using CQRS.Operations;
using CQRS.Repositories;

namespace CQRS.Handlers
{
    // this handler executes the command on the background 
    // you can imagine that these type of handlers do more than just storing the data, they can refresh caches
    // or post messages on some bus to inform services that these entities have changed and they need to
    // update their cache
    public class BackgroundAddCommentCommandHander : ICommandHandler<AddCommentCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public BackgroundAddCommentCommandHander(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new AddCommentOperation(command));
        }
    }
}
