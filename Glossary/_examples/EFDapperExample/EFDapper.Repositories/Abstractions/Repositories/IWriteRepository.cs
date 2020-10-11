using System.Threading.Tasks;
using EFDapper.Repositories.Abstractions.Operations;

namespace EFDapper.Repositories.Abstractions.Repositories
{
    public interface IWriteRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        Task CreateAsync(ICreationOperation<TEntity> creation);

        Task UpdateOneAsync(IUpdateOperation<TEntity> update);

        Task UpdateManyAsync(IUpdateOperation<TEntity> update);

        Task ExecuteAsync(ISqlOperation<TEntity> operation);
    }
}
