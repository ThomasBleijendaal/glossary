using System.Threading.Tasks;

namespace Mediator.Services
{
    public interface IUserService
    {
        Task DeleteAsync(string userId);
        Task IncreaseReputationAsync(string userId);
    }
}
