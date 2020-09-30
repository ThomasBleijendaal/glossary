namespace Repository
{
    public interface IMapper<TEntity, TModel>
    {
        TEntity Map(TModel model);
        TEntity Map(TEntity orignalEntity, TEntity newEntity);
        TModel Map(TEntity entity);
    }
}
