namespace EFDapper.Repositories.Abstractions.Specifications
{
    public interface IQueryExpressionSpecification<TEntity, TModel> : IExpressionSpecification<TEntity, TModel>
        where TEntity : IEntity
    {
        bool SplitQuery { get; }
    }
}
