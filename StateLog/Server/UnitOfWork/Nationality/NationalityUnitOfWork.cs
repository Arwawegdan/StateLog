namespace StateLog.Server;
public class NationalityUnitOfWork : INationalityUnitOfWork
{
    private readonly INationalityRepository _nationalityrepository; 
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly INationalityCosmosDbRepository _nationalityCosmosRepository;

    public NationalityUnitOfWork(INationalityRepository nationalityRepository, INationalityCosmosDbRepository  nationalityCosmosRepository, IStateLogCustomTagsRepository stateLogRepository)
    { 
        _nationalityrepository = nationalityRepository;
        _stateLogIndexingRepository = stateLogRepository;   
        _nationalityCosmosRepository = nationalityCosmosRepository;
    }
    public async Task<IEnumerable<Nationality>> Read() => await _nationalityCosmosRepository.Get("SELECT * FROM c");

    public async Task<Nationality> Read(Guid id) => await _nationalityCosmosRepository.Get(id);

    public async Task<IEnumerable<Nationality>> ReadByTagValue(string text)
    { 
        List<Nationality> entities = new List<Nationality>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagValue(text);
        foreach (Guid id in ids) entities.Add(await _nationalityCosmosRepository.Get(id));
        return entities;
    }
    public async Task<IEnumerable<Nationality>> ReadByTagName(string text)
    {
        List<Nationality> entities = new List<Nationality>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagName(text);
        foreach (Guid id in ids) entities.Add(await _nationalityCosmosRepository.Get(id));
        return entities;
    }
    public async Task Create(Nationality nationality)
    {
            StateLogCustomTags stateLogCustomTags = new StateLogCustomTags();
            stateLogCustomTags.Id = Guid.NewGuid();
            stateLogCustomTags.RowId = nationality.Id;
            stateLogCustomTags.TagName = nationality.TagName;
            stateLogCustomTags.TagValue = nationality.TagValue;
            stateLogCustomTags.BranchId = nationality.BranchId;
            stateLogCustomTags.CompanyId = nationality.CompanyId;
            stateLogCustomTags.ProductId = nationality.ProductId;
            stateLogCustomTags.EntityName = "nationality";

        using IDbContextTransaction transaction = _nationalityrepository.Context.Database.BeginTransaction();
        try
        {
            await _nationalityrepository.Add(nationality);
            await _stateLogIndexingRepository.Add(stateLogCustomTags);
            await _nationalityCosmosRepository.Add(nationality);
            
            transaction.Commit(); 
        } 
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();  
            throw;
        }
    }
    public async Task Create(IEnumerable<Nationality> entities)
    {
        foreach (Nationality entity in entities) await Create(entity);
    }

    public async Task Update(Nationality entity) => await _nationalityCosmosRepository.Update(entity);

    public async Task Update(List<Nationality> entities) => await _nationalityCosmosRepository.Update(entities);

    public async Task Delete(Nationality entity) => await _nationalityCosmosRepository.Delete(entity);

    public async Task Delete(Guid id)
    {
        Nationality entity = await _nationalityCosmosRepository.Get(id);
        await _nationalityCosmosRepository.Delete(entity);
    }

    public async Task Delete(IEnumerable<Nationality> entities) => await _nationalityCosmosRepository.Delete(entities);

}