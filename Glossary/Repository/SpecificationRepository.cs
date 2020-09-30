using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class SpecificationRepository<TEntity, TModel> : Repository<TEntity, TModel>, ISpecificationRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        public SpecificationRepository(ExampleDbContext dbContext, IMapper<TEntity, TModel> mapper) : base(dbContext, mapper)
        {
        }

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return includes.Aggregate(query, (query, include) => query.Include(include));
        }

        public async Task<TSpecModel> GetAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);
            return await query.Where(specification.Criteria).Select(specification.Projection).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TSpecModel>> GetListAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);
            return await query.Where(specification.Criteria).Select(specification.Projection).ToListAsync();
        }
    }
}
