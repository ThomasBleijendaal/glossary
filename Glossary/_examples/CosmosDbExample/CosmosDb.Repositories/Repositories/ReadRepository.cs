using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDb.Repositories.Abstractions;
using CosmosDb.Repositories.Abstractions.Repositories;
using CosmosDb.Repositories.Abstractions.Specifications;
using CosmosDb.Repositories.Extensions;
using MongoDB.Driver;

namespace CosmosDb.Repositories.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public ReadRepository(IMongoDatabase mongoDatabase)
        {
            _mongoCollection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task<TModel?> GetFirstAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            return await _mongoCollection
                .Find(specification.Criteria)
                .Project(specification.Projection)
                .OrderBys(specification.SortingInstructions)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification)
        {
            return await _mongoCollection
                .Find(specification.Criteria)
                .Project(specification.Projection)
                .OrderBys(specification.SortingInstructions)
                .ToListAsync();
        }
    }
}
