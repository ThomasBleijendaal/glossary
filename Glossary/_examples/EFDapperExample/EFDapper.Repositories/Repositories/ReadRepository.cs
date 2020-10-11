using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EFDapper.Repositories.Abstractions;
using EFDapper.Repositories.Abstractions.Repositories;
using EFDapper.Repositories.Abstractions.Specifications;
using EFDapper.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EFDapper.Repositories.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly AppDbContext _appDbContext;

        public ReadRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TModel?> GetFirstAsync<TModel>(ISpecification<TModel> specification)
        {
            if (specification is IExpressionSpecification<TEntity, TModel> expressionSpecification)
            {
                var query = ApplySpecification(expressionSpecification);
                return await PerformFirstOrDefaultAsync(query, expressionSpecification);
            }
            else if (specification is IProjectedSqlSpecification<TModel> projectionSpecification)
            {
                return (await PerformQueryAsync(projectionSpecification)).FirstOrDefault();
            }
            else if (specification is ISqlSpecification<TModel> sqlSpecification)
            {
                return await DbConnection.QueryFirstOrDefaultAsync<TModel>(
                    sqlSpecification.Sql,
                    sqlSpecification.Parameters);
            }

            throw new InvalidOperationException("Unsupported specification.");
        }

        public async Task<IReadOnlyList<TModel>> GetListAsync<TModel>(ISpecification<TModel> specification)
        {
            if (specification is IExpressionSpecification<TEntity, TModel> expressionSpecification)
            {
                var query = ApplySpecification(expressionSpecification);
                return await PerformToListAsync(query, expressionSpecification);
            }
            else if (specification is IProjectedSqlSpecification<TModel> projectedSpecification)
            {
                return (await PerformQueryAsync(projectedSpecification)).ToList();
            }
            else if (specification is ISqlSpecification<TModel> sqlSpecification)
            {
                var data = await DbConnection.QueryAsync<TModel>(
                    sqlSpecification.Sql,
                    sqlSpecification.Parameters);

                return data.ToList();
            }

            throw new InvalidOperationException("Unsupported specification.");
        }

        private IQueryable<TEntity> ApplySpecification<TModel>(IExpressionSpecification<TEntity, TModel> specification)
        {
            return _appDbContext
                .Set<TEntity>()
                .AsNoTracking()
                .Includes(specification.Includes)
                .Where(specification.Criteria)
                .OrderBys(specification.SortingInstructions);
        }

        private static async Task<TModel> PerformFirstOrDefaultAsync<TModel>(IQueryable<TEntity> query, IExpressionSpecification<TEntity, TModel> specification)
        {
            if (specification is not IQueryExpressionSpecification<TEntity, TModel> querySpecification)
            {
                return await query.Select(specification.Projection).FirstOrDefaultAsync();
            }
            else
            {
                if (querySpecification.SplitQuery)
                {
                    var projection = specification.Projection.Compile();
                    return projection(await query.AsSplitQuery().FirstOrDefaultAsync());
                }
                else
                {
                    return await query.AsSingleQuery().Select(specification.Projection).FirstOrDefaultAsync();
                }
            }
        }

        private static async Task<IReadOnlyList<TModel>> PerformToListAsync<TModel>(IQueryable<TEntity> query, IExpressionSpecification<TEntity, TModel> specification)
        {
            if (specification is not IQueryExpressionSpecification<TEntity, TModel> querySpecification)
            {
                return await query.Select(specification.Projection).ToListAsync();
            }
            else
            {
                if (querySpecification.SplitQuery)
                {
                    var projection = specification.Projection.Compile();
                    return (await query.AsSplitQuery().ToArrayAsync()).Select(projection).ToList();
                }
                else
                {
                    return await query.AsSingleQuery().Select(specification.Projection).ToListAsync();
                }
            }
        }

        private DbConnection DbConnection => _appDbContext.Database.GetDbConnection();

        private async Task<IEnumerable<TModel>> PerformQueryAsync<TModel>(IProjectedSqlSpecification<TModel> specification)
        {
            var data = await DbConnection.QueryAsync(specification.Sql, specification.Parameters);

            return data.Select(specification.Projection);
        }
    }
}
