using System.Threading.Tasks;
using CQRS.Commands;

namespace CQRS.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
