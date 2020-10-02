using System.Threading.Tasks;
using CQRS.Commands;

namespace CQRS.Services
{
    public interface ICommandQueuingService
    {
        Task QueueAsync(ICommand command);
    }
}
