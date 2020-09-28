using System.Threading.Tasks;

namespace CommandDispatcher
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
