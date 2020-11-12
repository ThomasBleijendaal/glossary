using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.Cosmos.Table;

namespace CQRS.Extensions
{
    public static class TableQueryExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> query,
            IEnumerable<Sort<TEntity>>? sortingInstructions)
        {
            if (query is not TableQuery<TEntity> tableQuery ||
                sortingInstructions?.Any() != true)
            {
                return query;
            }

            var firstKeySelector = sortingInstructions.First();

            var memberExpression = default(MemberExpression);

            if (firstKeySelector.KeySelector.Body is MemberExpression member)
            {
                memberExpression = member;
            }
            else if (firstKeySelector.KeySelector.Body is UnaryExpression unaryExpression &&
                unaryExpression.Operand is MemberExpression operandMember)
            {
                memberExpression = operandMember;
            }

            if (memberExpression == null)
            {
                throw new InvalidOperationException("Invalid key selector in sort (Must be member expression).");
            }

            if (firstKeySelector.SortingDirection == SortingDirection.Ascending)
            {
                return tableQuery.OrderBy(memberExpression.Member.Name);
            }
            else
            {
                return tableQuery.OrderByDesc(memberExpression.Member.Name);
            }
        }
    }
}
