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
    public class BackgroundAddLikeCommandHander : ICommandHandler<AddLikeCommand>
    {
        private readonly IWriteRepository<BlogPost> _repository;

        public BackgroundAddLikeCommandHander(IWriteRepository<BlogPost> repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(AddLikeCommand command)
        {
            await _repository.UpdateSingleEntityAsync(new AddLikeOperation(command));
        }
    }
}
