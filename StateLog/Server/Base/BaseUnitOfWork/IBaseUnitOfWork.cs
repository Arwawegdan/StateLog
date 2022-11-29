namespace StateLog.Server;
public interface IBaseUnitOfWork<TEntity> : IDisposable where TEntity : BaseEntity
{
    Task Create(TEntity entity);
    Task Create(IEnumerable<TEntity> entities);

    Task<IEnumerable<TEntity>> Read();
    Task<TEntity> Read(Guid id);

    Task Update(TEntity entity);
    Task Update(List<TEntity> entities);

    Task Delete(Guid id);
    Task Delete(TEntity entity);
    Task Delete(IEnumerable<TEntity> entities);
}