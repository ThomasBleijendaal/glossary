using System.Collections.Generic;
using System.Threading.Tasks;
using CosmosDb.Repositories.Abstractions.Specifications;

namespace CosmosDb.Repositories.Abstractions.Repositories
{
    public interface IReadRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<TModel?> GetFirstAsync<TModel>(ISpecification<TEntity, TModel> specification);
        Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TEntity, TModel> specification);
    }
}
