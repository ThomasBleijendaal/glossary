using System.Collections.Generic;
using System.Linq;
using EFDapper.Repositories.Enums;
using EFDapper.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace EFDapper.Repositories.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> Includes<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<string>? includes)
            where TEntity : class
        {
            if (includes == null)
            {
                return queryable;
            }

            return includes
                .Aggregate(
                    queryable,
                    (queryable, include) => queryable.Include(include));
        }

        public static IQueryable<TEntity> OrderBys<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<Sort<TEntity>>? orderBys)
        {
            if (orderBys == null)
            {
                return queryable;
            }

            var firstSort = orderBys.First();

            return orderBys
                .Skip(1)
                .Aggregate(
                    firstSort.SortingDirection == SortingDirection.Ascending
                        ? queryable.OrderBy(firstSort.KeySelector)
                        : queryable.OrderByDescending(firstSort.KeySelector),
                    (orderedQueryable, nextSort) =>
                        nextSort.SortingDirection == SortingDirection.Ascending
                            ? orderedQueryable.ThenBy(nextSort.KeySelector)
                            : orderedQueryable.ThenByDescending(nextSort.KeySelector));
        }
    }
}
