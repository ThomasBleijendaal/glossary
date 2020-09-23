using System.Threading.Tasks;

namespace CommandDispatcher
{
    class CommandB2Handler : ICommandHandler<CommandB, Result2>
    {
        public Task<Result2> Handle(CommandB command)
        {
            return Task.FromResult(new Result2());
        }
    }
}
