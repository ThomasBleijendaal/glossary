using System.Threading.Tasks;

namespace CommandDispatcher.Dispatchers
{
    public interface ICommandDispatcher<TBaseComand, TBaseResult>
    {
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : TBaseComand
            where TResult : TBaseResult;
    }
}
