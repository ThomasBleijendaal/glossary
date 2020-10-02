using System;

namespace CQRS.Creations
{
    public interface ICreation<TEntity>
    {
        Action<TEntity> Mutation { get; }

        int CreatedId { set; }
    }
}
