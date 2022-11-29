namespace StateLog.Server;
public interface IBaseSettingsUnitOfWork<TEntity> : IBaseUnitOfWork<TEntity> , IDisposable 
    where TEntity : BaseSettingsEntity
{
    Task<IEnumerable<TEntity>> Search(string searchText);
}