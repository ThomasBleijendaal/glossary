using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pipeline
{
    public interface IPipelineActions
    {
        Task<int> Multiplier(int input);
        Task<int> Aggregator(IEnumerable<int> input);
        Task Outputer(int input);
    }
}
