using System;

namespace Descriptor
{
    public class EntityDescriptor
    {
        public Type EntityType { get; set; }
        public Guid Id { get; set; }

        public EntityDescriptor Parent { get; set; }
    }
}
