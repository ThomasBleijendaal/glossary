using System.Collections.Generic;
using System.Threading.Tasks;
using EFDapper.Repositories.Abstractions.Specifications;

namespace EFDapper.Repositories.Abstractions.Repositories
{
    public interface IReadRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<TModel?> GetFirstAsync<TModel>(ISpecification<TModel> specification);
        Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TModel> specification);
    }
}
