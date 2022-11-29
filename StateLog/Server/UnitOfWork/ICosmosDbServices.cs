namespace StateLog.Server;
public interface ICosmosDbServices<TEntity> 
    where TEntity : BaseEntity
{
    Task<TEntity> Add(TEntity entity);
    Task<IEnumerable<TEntity>> Add(IEnumerable<TEntity> entities);

    Task<IEnumerable<TEntity>> Get(string queryString);
    Task<TEntity> Get(Guid id);

    Task<TEntity> Update(TEntity entity);
   
    Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entities);

    Task<TEntity> Delete(TEntity entity);
    Task<IEnumerable<TEntity>> Delete(IEnumerable<TEntity> entities);
}