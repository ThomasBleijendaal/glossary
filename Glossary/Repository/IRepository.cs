using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Task<TModel> GetAsync(int id);
        Task<IEnumerable<TModel>> GetListAsync();
        Task AddAsync(TModel entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(TModel entity);
        Task UpdateAsync(TModel entity);
    }
}
