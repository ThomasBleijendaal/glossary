using System.Threading.Tasks;
using Mediator.Events;

namespace Mediator.Mediators
{
    public interface IMediator
    {
        Task NotifyAsync(IMediatorComponent sender, IMediationEvent @event);
    }
}
