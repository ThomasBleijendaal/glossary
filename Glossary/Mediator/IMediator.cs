using System.Threading.Tasks;

namespace Mediator
{
    public interface IMediator
    {
        Task NotifyAsync(IMediatorComponent sender, IMediationEvent @event);
    }
}
