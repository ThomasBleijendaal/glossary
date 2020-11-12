using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Extensions;
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
                .Where(specification.Criteria)
                .OrderBy(specification.SortingInstructions);

            var data = query.FirstOrDefault();

            // async?
            return specification.Projection.Invoke(data);
        }

        public async Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            var table = await GetTable();

            var query = table.CreateQuery<TEntity>()
                .Where(specification.Criteria)
                .OrderBy(specification.SortingInstructions);

            var data = query.ToList();

            // async?
            return data.Select(specification.Projection).ToList();
        }

        private async Task<CloudTable> GetTable()
        {
            var table = _client.GetTableReference(typeof(TEntity).Name);

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
