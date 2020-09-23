using System.Threading.Tasks;

namespace CommandDispatcher
{
    class CommandB1Handler : ICommandHandler<CommandB, Result1>
    {
        public Task<Result1> Handle(CommandB command)
        {
            return Task.FromResult(new Result1());
        }
    }
}
