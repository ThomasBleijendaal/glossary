using System.Collections.Generic;
using System.Linq;
using CosmosDb.Repositories.Enums;
using CosmosDb.Repositories.Models;
using MongoDB.Driver;

namespace CosmosDb.Repositories.Extensions
{
    public static class MongoDBExtensions
    {
        public static IFindFluent<TEntity, TModel> OrderBys<TEntity, TModel>(this IFindFluent<TEntity, TModel> findFluent, IEnumerable<Sort<TEntity>>? orderBys)
        {
            if (orderBys == null)
            {
                return findFluent;
            }

            var firstSort = orderBys.First();

            return orderBys
                .Skip(1)
                .Aggregate(
                    firstSort.SortingDirection == SortingDirection.Ascending
                        ? findFluent.SortBy(firstSort.KeySelector)
                        : findFluent.SortByDescending(firstSort.KeySelector),
                    (orderedFindFluent, nextSort) =>
                        nextSort.SortingDirection == SortingDirection.Ascending
                            ? orderedFindFluent.ThenBy(nextSort.KeySelector)
                            : orderedFindFluent.ThenByDescending(nextSort.KeySelector));
        }
    }
}
