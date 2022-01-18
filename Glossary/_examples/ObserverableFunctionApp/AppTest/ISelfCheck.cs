using System.Threading.Tasks;

namespace ObserverableFunctionApp.AppTest;

public interface ISelfCheck
{
    Task<Status> CheckAsync();
}
