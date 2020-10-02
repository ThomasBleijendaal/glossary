using System.Threading.Tasks;
using CQRS.Creations;
using CQRS.Operations;
using Microsoft.Azure.Cosmos.Table;

namespace CQRS.Repositories
{
    public interface IWriteRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        Task CreateEntityAsync(ICreation<TEntity> creation);
        Task UpdateSingleEntityAsync(IOperation<TEntity> operation);
        Task UpdateMultipleEntitiesAsync(IOperation<TEntity> operation);
        Task DeleteEntityAsync(int id);
    }
}
