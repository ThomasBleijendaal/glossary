using System.Threading.Tasks;

namespace ObserverableFunctionApp.Services;

public interface IScopedService
{
    Task DoSometingAsync();
}
