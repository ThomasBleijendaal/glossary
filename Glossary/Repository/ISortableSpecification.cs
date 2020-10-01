using System.Collections.Generic;

namespace Repository
{
    public interface ISortableSpecification<TEntity, TModel> : ISpecification<TEntity, TModel>
        where TEntity : BaseEntity
    {
        IEnumerable<Sort<TEntity>> SortingInstructions { get; }
    }
}
