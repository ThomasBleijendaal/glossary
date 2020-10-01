using System.Collections.Generic;
using System.Data;
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

        public async Task<TSpecModel> GetAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);
            var specQuery = query
                .Where(specification.Criteria);

            if (specification is ISortableSpecification<TEntity, TSpecModel> sortable)
            {
                specQuery = ApplySorting(query, sortable);
            }

            return await specQuery.Select(specification.Projection).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TSpecModel>> GetListAsync<TSpecModel>(ISpecification<TEntity, TSpecModel> specification)
        {
            var query = ApplyIncludes(_dbContext.Set<TEntity>(), specification.Includes);

            var specQuery = query.Where(specification.Criteria);

            if (specification is ISortableSpecification<TEntity, TSpecModel> sortable)
            {
                specQuery = ApplySorting(query, sortable);
            }

            return await specQuery.Select(specification.Projection).ToListAsync();
        }
        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, IEnumerable<string> includes)
        {
            return includes.Aggregate(query, (aggregateQuery, include) => aggregateQuery.Include(include));
        }

        private IQueryable<TEntity> ApplySorting<TSpecModel>(IQueryable<TEntity> query, ISortableSpecification<TEntity, TSpecModel> specification)
        {
            if (!specification.SortingInstructions.Any())
            {
                return query;
            }

            var firstKeySelector = specification.SortingInstructions.First();
            var orderedQuery = firstKeySelector.SortingDirection == SortingDirection.Ascending
                ? query.OrderBy(firstKeySelector.KeySelector)
                : query.OrderByDescending(firstKeySelector.KeySelector);

            return specification.SortingInstructions
                .Skip(1)
                .Aggregate(
                    orderedQuery,
                    (aggregateOrderedQuery, keySelector) => keySelector.SortingDirection == SortingDirection.Ascending
                        ? aggregateOrderedQuery.ThenBy(keySelector.KeySelector)
                        : aggregateOrderedQuery.ThenByDescending(keySelector.KeySelector));
        }
    }
}
