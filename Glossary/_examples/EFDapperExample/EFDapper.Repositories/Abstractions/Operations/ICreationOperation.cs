using System;

namespace EFDapper.Repositories.Abstractions.Operations
{
    public interface ICreationOperation<TEntity> : IOperation
    {
        Action<TEntity> Mutation { get; }

        int CreatedId { set; }
    }
}
