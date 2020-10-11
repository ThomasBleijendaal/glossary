using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFDapper.Repositories.Models;

namespace EFDapper.Repositories.Abstractions.Operations
{
    public interface IUpdateOperation<TEntity> : IOperation
    {
        Expression<Func<TEntity, bool>> Criteria { get; }

        IEnumerable<Sort<TEntity>>? SortingInstructions { get; }

        Action<TEntity> Mutation { get; }

        IEnumerable<int> UpdatedIds { set; }
    }
}
