using System.Threading.Tasks;
using CosmosDb.Repositories.Abstractions.Operations;

namespace CosmosDb.Repositories.Abstractions.Repositories
{
    public interface IWriteRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        Task CreateAsync(ICreationOperation<TEntity> creation);

        Task UpdateOneAsync(IUpdateOperation<TEntity> update);

        Task UpdateManyAsync(IUpdateOperation<TEntity> update);
    }
}
