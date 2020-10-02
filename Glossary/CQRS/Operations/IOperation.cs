using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Operations
{
    public interface IOperation<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
        Action<TEntity> Mutation { get; }
        Func<TEntity, bool> Validation { get; }
    }
}
