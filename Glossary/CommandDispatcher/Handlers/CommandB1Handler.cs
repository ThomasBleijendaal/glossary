using System.Threading.Tasks;
using CommandDispatcher.Models;

namespace CommandDispatcher.Handlers
{
    public class CommandB1Handler : ICommandHandler<CommandB, Result1>
    {
        public Task<Result1> Handle(CommandB command)
        {
            return Task.FromResult(new Result1());
        }
    }
}
