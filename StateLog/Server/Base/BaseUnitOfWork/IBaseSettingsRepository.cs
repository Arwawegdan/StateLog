namespace StateLog.Server;
public interface IBaseSettingsRepository<TEntity> : IBaseRepository<TEntity>, IDisposable
    where TEntity : BaseSettingsEntity
{
    Task<IEnumerable<TEntity>> Search(string searchText);
}