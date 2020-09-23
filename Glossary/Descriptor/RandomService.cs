using System.Threading.Tasks;

namespace Descriptor
{
    class RandomService
    {
        public Task SomeRandomAction(EntityDescriptor entityDescriptor)
        {
            return Task.CompletedTask;
        }
    }
}
