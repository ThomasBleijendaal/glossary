using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISpecificationRepository<TEntity, TModel> : IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        Task<TSpecModel> GetAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification);
        Task<IEnumerable<TSpecModel>> GetListAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification);
    }
}
