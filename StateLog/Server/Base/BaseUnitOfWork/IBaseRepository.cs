namespace StateLog.Server;
public interface IBaseRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    Task Add(TEntity entity);
    Task Add(IEnumerable<TEntity> entities);

    Task<IEnumerable<TEntity>> Get();
    Task<TEntity> Get(Guid id);

    Task Update(TEntity entity, bool isAutosSaveChangesEnabled = true);
    Task Update(List<TEntity> entities);

    Task Remove(Guid id);
    Task Remove(TEntity entity);
    Task Remove(IEnumerable<TEntity> entities);

    Task<IDbContextTransaction> GetTransaction();
}