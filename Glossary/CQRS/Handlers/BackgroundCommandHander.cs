using System.Threading.Tasks;
using CQRS.Commands;
using CQRS.Entities;
using CQRS.Operations;
using CQRS.Repositories;

namespace CQRS.Handlers
{
    // this handler executes these commands on the background 
    // you can imagine that these handlers do more than just storing the data, they can refresh caches
    // or post messages on some bus to inform services that these entities have changed and they need to
    // update their cache
    public class BackgroundCommandHander :
        ICommandHandler<AddLikeCommand>,
        ICommandHandler<AddCommentCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public BackgroundCommandHander(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(AddLikeCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new AddLikeOperation(command));
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new AddCommentOperation(command));
        }

    }

}
