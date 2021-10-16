using System.Threading.Tasks;
using CommandDispatcher.Models;

namespace CommandDispatcher.Handlers
{
    public class CommandA1Handler : ICommandHandler<CommandA, Result1>
    {
        public Task<Result1> Handle(CommandA command)
        {
            return Task.FromResult(new Result1());
        }
    }
}
