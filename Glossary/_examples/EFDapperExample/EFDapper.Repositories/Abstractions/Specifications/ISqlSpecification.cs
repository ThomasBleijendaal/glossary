namespace EFDapper.Repositories.Abstractions.Specifications
{
    public interface ISqlSpecification<TModel> : ISpecification<TModel>
    {
        string Sql { get; }

        object? Parameters { get; }
    }
}
