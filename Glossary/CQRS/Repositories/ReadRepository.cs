using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CQRS.Specifications;
using Microsoft.Azure.Cosmos.Table;

namespace CQRS.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        private readonly CloudTableClient _client;

        public ReadRepository(CloudStorageAccount cloudStorageAccount)
        {
            _client = cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task<TModel> GetAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            var table = await GetTable();

            var query = table.CreateQuery<TEntity>()
                .Where(specification.Criteria);

            //if (specification.SortingInstructions != null)
            //{
            //    query = ApplySorting(query, specification);
            //}

            var data = query.FirstOrDefault();

            // async?
            return specification.Projection.Compile().Invoke(data);
        }

        public async Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            var table = await GetTable();

            var query = table.CreateQuery<TEntity>()
                .Where(specification.Criteria);

            //if (specification.SortingInstructions != null)
            //{
            //    query = ApplySorting(query, specification);
            //}

            var data = query.ToList();

            // async?
            return data.Select(specification.Projection.Compile()).ToList();
        }

        private async Task<CloudTable> GetTable()
        {
            var table = _client.GetTableReference(typeof(TEntity).Name);

            await table.CreateIfNotExistsAsync();

            return table;
        }

        private IQueryable<TEntity> ApplySorting<TModel>(TableQuery<TEntity> query, ISpecification<TEntity, TModel> specification)
        {
            if (!specification.SortingInstructions.Any())
            {
                return query;
            }
            else if (specification.SortingInstructions.Count() > 1)
            {
                throw new InvalidOperationException("Only one sort is allowed.");
            }

            var firstKeySelector = specification.SortingInstructions.First();
            if (!(firstKeySelector.KeySelector is ParameterExpression parameter))
            {
                throw new InvalidOperationException("Invalid key selector in sort.");
            }

            var orderedQuery = firstKeySelector.SortingDirection == SortingDirection.Ascending
                ? query.OrderBy(parameter.Name)
                : query.OrderByDesc(parameter.Name);

            return orderedQuery;
        }
    }
}
