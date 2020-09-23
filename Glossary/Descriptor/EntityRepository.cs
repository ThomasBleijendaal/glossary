﻿using System.Threading.Tasks;

namespace Descriptor
{
    class EntityRepository
    {
        public Task<Entity> GetEntityByDescriptor(EntityDescriptor entityDescriptor)
        {
            // repo logic which will fetch the entity
            return Task.FromResult(new Entity { Id = entityDescriptor.Id });
        }
    }
}
