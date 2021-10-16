using System;

namespace Descriptor
{
    public class Entity
    {
        public Guid Id { get; set; }
        
        public EntityDescriptor ToEntityDescriptor => new EntityDescriptor { EntityType = GetType(), Id = Id };
    }
}
