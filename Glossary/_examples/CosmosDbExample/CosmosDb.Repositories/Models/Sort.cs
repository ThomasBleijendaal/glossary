using System;
using System.Linq.Expressions;
using CosmosDb.Repositories.Enums;

namespace CosmosDb.Repositories.Models
{
    public struct Sort<TEntity>
    {
        public SortingDirection SortingDirection { get; set; }
        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }
}
