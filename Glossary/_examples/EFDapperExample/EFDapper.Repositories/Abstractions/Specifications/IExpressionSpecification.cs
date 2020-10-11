using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFDapper.Repositories.Models;

namespace EFDapper.Repositories.Abstractions.Specifications
{
    public interface IExpressionSpecification<TEntity, TModel> : ISpecification<TModel>
        where TEntity : IEntity
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        IEnumerable<string>? Includes { get; }
        Expression<Func<TEntity, TModel>> Projection { get; }
        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }
    }
}
