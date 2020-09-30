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

    public interface ISortableSpecification<TEntity, TModel> : ISpecification<TEntity, TModel>
        where TEntity : BaseEntity
    {
        IEnumerable<Sort<TEntity>> KeySelectors { get; }
    }

    public enum SortingDirection
    {
        Ascending,
        Descending
    }

    public struct Sort<TEntity>
    {
        public SortingDirection SortingDirection { get; set; }
        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }
}
