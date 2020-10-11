namespace EFDapper.Repositories.Abstractions.Operations
{
    public interface ISqlOperation<TEntity> : IOperation
    {
        string Sql { get; }

        object? Parameters { get; }
    }
}
