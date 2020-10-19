using System.Threading.Tasks;
using CQRS.Commands;

namespace CQRS.Handlers
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
