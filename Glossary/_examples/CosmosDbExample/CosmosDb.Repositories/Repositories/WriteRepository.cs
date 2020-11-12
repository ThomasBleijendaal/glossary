using System.Threading.Tasks;
using CosmosDb.Repositories.Abstractions;
using CosmosDb.Repositories.Abstractions.Operations;
using CosmosDb.Repositories.Abstractions.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CosmosDb.Repositories.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public WriteRepository(IMongoDatabase mongoDatabase)
        {
            _mongoCollection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task CreateAsync(ICreationOperation<TEntity> creation)
        {
            var entity = new TEntity
            {
                Id = new ObjectId()
            };

            creation.Mutation.Invoke(entity);

            await _mongoCollection.InsertOneAsync(entity);

            creation.CreatedId = entity.Id.ToString();
        }

        public async Task UpdateManyAsync(IUpdateOperation<TEntity> update)
            => await _mongoCollection.UpdateManyAsync(update.Criteria, CreateUpdateDefinition(update));

        public async Task UpdateOneAsync(IUpdateOperation<TEntity> update)
            => await _mongoCollection.UpdateOneAsync(update.Criteria, CreateUpdateDefinition(update));

        private static UpdateDefinition<TEntity> CreateUpdateDefinition(IUpdateOperation<TEntity> update)
            => update.Mutation.Invoke(new UpdateDefinitionBuilder<TEntity>());
    }
}
