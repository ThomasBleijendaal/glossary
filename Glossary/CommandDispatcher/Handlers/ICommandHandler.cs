using System.Threading.Tasks;
using CommandDispatcher.Dispatchers;
using CommandDispatcher.Models;

namespace CommandDispatcher.Handlers
{
    public interface ICommandHandler<TCommandDispatcher>
    {

    }

    public interface ICommandHandler<TCommand, TResult> : ICommandHandler<IExampleCommandDispatcher>
        where TCommand : BaseCommand
        where TResult : BaseResult

    {
        Task<TResult> Handle(TCommand command);
    }
}
