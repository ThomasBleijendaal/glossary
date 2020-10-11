using System;

namespace EFDapper.Repositories.Abstractions.Specifications
{
    public interface IProjectedSqlSpecification<TModel> : ISqlSpecification<TModel>
    {
        Func<dynamic, TModel> Projection { get; }
    }
}
