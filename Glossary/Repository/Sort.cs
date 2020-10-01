using System;
using System.Linq.Expressions;

namespace Repository
{
    public struct Sort<TEntity>
    {
        public SortingDirection SortingDirection { get; set; }
        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }
}
