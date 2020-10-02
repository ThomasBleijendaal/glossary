using System;

namespace CQRS.Commands
{
    public interface ICommand
    {
        Guid CommandId { get; set; }
    }
}
