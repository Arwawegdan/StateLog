namespace StateLog.Server;
public class BaseSettingsUnitOfWork<TEntity> : BaseUnitOfWork<TEntity>, IBaseSettingsUnitOfWork<TEntity>
    where TEntity : BaseSettingsEntity
{
    private readonly IBaseSettingsRepository<TEntity> _baseSettingsRepository;
    public BaseSettingsUnitOfWork(IBaseSettingsRepository<TEntity> repository) : base(repository) => 
        _baseSettingsRepository = repository;

    public async Task<IEnumerable<TEntity>> Search(string searchText) => await _baseSettingsRepository.Search(searchText);
}