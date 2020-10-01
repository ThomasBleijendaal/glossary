using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository
{
    public interface ISpecification<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<string> Includes { get; }
        Expression<Func<TEntity, TModel>> Projection { get; }
    }
}
