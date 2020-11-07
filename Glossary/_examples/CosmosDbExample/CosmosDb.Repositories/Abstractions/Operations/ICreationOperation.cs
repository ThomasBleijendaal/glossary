using System;

namespace CosmosDb.Repositories.Abstractions.Operations
{
    public interface ICreationOperation<TEntity> : IOperation
    {
        Action<TEntity> Mutation { get; }

        string CreatedId { set; }
    }
}
