using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<TEntity, TModel> : IRepository<TEntity, TModel>
        where TEntity : BaseEntity
    {
        protected readonly ExampleDbContext _dbContext;
        protected readonly IMapper<TEntity, TModel> _mapper;

        public Repository(ExampleDbContext dbContext, IMapper<TEntity, TModel> mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddAsync(TModel entity)
        {
            _dbContext.Set<TEntity>().Add(_mapper.Map(entity));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _dbContext.Remove(await _dbContext.Set<TEntity>().Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync() ?? throw new Exception("This should be a not found exception."));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TModel entity)
        {
            _dbContext.Remove(_mapper.Map(entity));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetListAsync()
        {
            var entities = await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            return entities.Select(_mapper.Map);
        }

        public async Task<TModel> GetAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().Where(x => x.Id == id).AsNoTracking().SingleOrDefaultAsync();
            if (entity == null)
            {
                throw new Exception("This should be a not found exception");
            }

            return _mapper.Map(entity);
        }

        public async Task UpdateAsync(TModel entity)
        {
            var updatedEntity = _mapper.Map(entity);
            var originalEntity = await _dbContext.Set<TEntity>().FindAsync(updatedEntity.Id) ?? throw new Exception("This should be a not found exception");

            var mergedEntity = _mapper.Map(originalEntity, updatedEntity);

            _dbContext.Set<TEntity>().Update(mergedEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
