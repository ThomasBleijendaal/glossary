using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EFDapper.Repositories.Abstractions;
using EFDapper.Repositories.Abstractions.Operations;
using EFDapper.Repositories.Abstractions.Repositories;
using EFDapper.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFDapper.Repositories.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly AppDbContext _appDbContext;

        public WriteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateAsync(ICreationOperation<TEntity> creation)
        {
            var entity = new TEntity();
            var entry = _appDbContext.Set<TEntity>().Add(entity);

            var entitiesCreated = await _appDbContext.SaveChangesAsync();
            if (entitiesCreated == 1)
            {
                creation.CreatedId = entry.Entity.Id;
            }
        }

        public async Task UpdateOneAsync(IUpdateOperation<TEntity> update)
        {
            var entity = await ApplyOperation(update).FirstOrDefaultAsync();

            update.Mutation(entity);

            await _appDbContext.SaveChangesAsync();

            update.UpdatedIds = new[] { entity.Id };
        }

        public async Task UpdateManyAsync(IUpdateOperation<TEntity> update)
        {
            var entities = await ApplyOperation(update).ToListAsync();

            foreach (var entity in entities)
            {
                update.Mutation(entity);
            }

            await _appDbContext.SaveChangesAsync();

            update.UpdatedIds = entities.Select(x => x.Id);
        }

        private IQueryable<TEntity> ApplyOperation(IUpdateOperation<TEntity> update)
        {
            return _appDbContext
                .Set<TEntity>()
                .Where(update.Criteria)
                .OrderBys(update.SortingInstructions)
                .AsTracking();
        }

        public async Task ExecuteAsync(ISqlOperation<TEntity> operation)
        {
            await DbConnection.ExecuteAsync(
                operation.Sql,
                operation.Parameters);
        }

        private DbConnection DbConnection => _appDbContext.Database.GetDbConnection();
    }
}
