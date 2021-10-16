using System.Threading.Tasks;
using CommandDispatcher.Models;

namespace CommandDispatcher.Handlers
{
    public class CommandB2Handler : ICommandHandler<CommandB, Result2>
    {
        public Task<Result2> Handle(CommandB command)
        {
            return Task.FromResult(new Result2());
        }
    }
}
