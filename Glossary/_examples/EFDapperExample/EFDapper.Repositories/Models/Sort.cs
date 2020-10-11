using System;
using System.Linq.Expressions;
using EFDapper.Repositories.Enums;

namespace EFDapper.Repositories.Models
{
    public struct Sort<TEntity>
    {
        public SortingDirection SortingDirection { get; set; }
        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }
}
