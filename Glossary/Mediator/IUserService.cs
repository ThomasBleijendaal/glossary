using System.Threading.Tasks;

namespace Mediator
{
    public interface IUserService
    {
        Task DeleteAsync(string userId);
        Task IncreaseReputationAsync(string userId);
    }
}
