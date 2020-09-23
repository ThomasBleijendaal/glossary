using System;

namespace Descriptor
{
    class EntityDescriptor
    {
        public Type EntityType { get; set; }
        public Guid Id { get; set; }

        public EntityDescriptor Parent { get; set; }
    }
}
