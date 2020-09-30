using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Pipeline
{
    public class Pipeline<T>
    {
        public ITargetBlock<T> Input { get; set; }
        public Task Completion { get; set; }
    }
}
