using System;

namespace Descriptor
{
    class Entity
    {
        public Guid Id { get; set; }
        // [the actual entity]

        public EntityDescriptor ToEntityDescriptor => new EntityDescriptor { EntityType = GetType(), Id = Id };
    }
}
