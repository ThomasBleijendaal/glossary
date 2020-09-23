using System.Threading.Tasks;

namespace CommandDispatcher
{
    interface ICommandHandler<TCommandDispatcher>
    {

    }

    interface ICommandHandler<TCommand, TResult> : ICommandHandler<IExampleCommandDispatcher>
        where TCommand : BaseCommand
        where TResult : BaseResult

    {
        Task<TResult> Handle(TCommand command);
    }
}
