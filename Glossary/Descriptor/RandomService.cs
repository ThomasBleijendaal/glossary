﻿using System.Threading.Tasks;

namespace Descriptor
{
    public class RandomService
    {
        public Task SomeRandomAction(EntityDescriptor entityDescriptor)
        {
            return Task.CompletedTask;
        }
    }
}
