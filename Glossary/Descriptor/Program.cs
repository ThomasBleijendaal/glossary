/**
 * A descriptor is a model that carries information about something else.
 * For example, it describes an entity correctly without containing more than
 * it's id and type. Its easily serializable and could be used in an API.
 * 
 * */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Descriptor
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args )
        {
            await Init<Program, DescriptorApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddSingleton<RandomService>();
            services.AddSingleton<EntityRepository>();

        }

        public class DescriptorApp : BaseApp
        {
            private readonly EntityRepository _repository;
            private readonly RandomService _service;

            public DescriptorApp(EntityRepository repository, RandomService service)
            {
                _repository = repository;
                _service = service;
            }

            public override async Task Run()
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
}
