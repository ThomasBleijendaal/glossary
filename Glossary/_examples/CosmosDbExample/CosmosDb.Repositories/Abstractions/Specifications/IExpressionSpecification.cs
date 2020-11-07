using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CosmosDb.Repositories.Models;

namespace CosmosDb.Repositories.Abstractions.Specifications
{
    public interface ISpecification<TEntity, TModel>
        where TEntity : IEntity
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, TModel>> Projection { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
    }
}
