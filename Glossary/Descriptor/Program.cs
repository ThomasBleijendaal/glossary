/**
 * A descriptor is a model that carries information about something else.
 * For example, it describes an entity correctly without containing more than
 * it's id and type. Its easily serializable and could be used in an API.
 * 
 * */

using System;
using System.Threading.Tasks;

namespace Descriptor
{
    class Program
    {
        static RandomService _service = new RandomService();
        static EntityRepository _repository = new EntityRepository();

        static async Task Main(string[] args)
        {
            // gets the descriptor from some process or API
            var descriptor = new EntityDescriptor { Id = Guid.NewGuid(), EntityType = typeof(Entity) };

            // uses the descriptor to fetch the real entity
            var entity = await _repository.GetEntityByDescriptor(descriptor);

            Console.WriteLine(entity.Id);

            // or uses the descriptor to do something with it, without passing the entire entity around
            await _service.SomeRandomAction(descriptor);
        }
    }
}
