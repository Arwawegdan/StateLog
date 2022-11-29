namespace StateLog.Server;
public class StateLogCustomTagsUnitOfWork : BaseUnitOfWork<StateLogCustomTags>, IStateLogCustomTagsUnitOfWork
{
    private readonly IStateLogCustomTagsRepository _repository;
    public StateLogCustomTagsUnitOfWork(IStateLogCustomTagsRepository stateLogCustomTagsRepository) 
                    : base(stateLogCustomTagsRepository)
    {
        _repository = stateLogCustomTagsRepository;      
    }
    public async Task<IEnumerable<Guid?>> GetUniqueValues(IEnumerable<Guid?> ListBeforeFilteration)
                                                                => ListBeforeFilteration.ToHashSet().ToList();
    public async Task<IEnumerable<Guid?>> SearchByTagValue(string text)
    {
        IEnumerable<Guid?> guidsFromSqlDatabase = await _repository.SearchByTagValue(text);
        return await GetUniqueValues(guidsFromSqlDatabase);
    }
    public async Task<IEnumerable<Guid?>> SearchByTagName(string text)
    {
        IEnumerable<Guid?> guidsFromSqlDatabase = await _repository.SearchByTagName(text);
        return await GetUniqueValues(guidsFromSqlDatabase);
    }
    public async Task<IEnumerable<Guid?>> SearchByEntityName(string text)
    {
        IEnumerable<Guid?> guidsFromSqlDatabase = await _repository.SearchByEntityName(text);
        return await GetUniqueValues(guidsFromSqlDatabase);
    }
}