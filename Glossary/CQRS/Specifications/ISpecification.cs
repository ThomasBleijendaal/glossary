using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CQRS.Specifications
{
    public interface ISpecification<TEntity, TModel>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Func<TEntity, TModel> Projection { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
    }
}
