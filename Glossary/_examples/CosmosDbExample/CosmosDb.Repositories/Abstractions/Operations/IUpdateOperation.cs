using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosmosDb.Repositories.Models;
using MongoDB.Driver;

namespace CosmosDb.Repositories.Abstractions.Operations
{
    public interface IUpdateOperation<TEntity> : IOperation
    {
        Expression<Func<TEntity, bool>> Criteria { get; }

        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }

        Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> Mutation { get; }
    }
}
