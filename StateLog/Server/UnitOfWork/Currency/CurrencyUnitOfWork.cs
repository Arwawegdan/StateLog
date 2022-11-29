namespace StateLog.Server;
public class CurrencyUnitOfWork : ICurrencyUnitOfWork
{
    private readonly ICurrencyRepository _Currencyrepository; 
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly ICurrencyCosmosDbRepository _CurrencyCosmosRepository;

    public CurrencyUnitOfWork(ICurrencyRepository CurrencyRepository, ICurrencyCosmosDbRepository  CurrencyCosmosRepository)
    { 
        _Currencyrepository = CurrencyRepository;
        _stateLogIndexingRepository = new StateLogCustomTagsRepository(CurrencyRepository.Context);   
        _CurrencyCosmosRepository = CurrencyCosmosRepository;
    }
    public async Task<IEnumerable<Currency>> Read() => await _CurrencyCosmosRepository.Get("SELECT * FROM c");

    public async Task<Currency> Read(Guid id) => await _CurrencyCosmosRepository.Get(id);

    public async Task<IEnumerable<Currency>> ReadByTagValue(string text)
    { 
        List<Currency> entities = new List<Currency>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagValue(text);
        foreach (Guid id in ids) entities.Add(await _CurrencyCosmosRepository.Get(id));
        return entities;
    }
    public async Task<IEnumerable<Currency>> ReadByTagName(string text)
    {
        List<Currency> entities = new List<Currency>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagName(text);
        foreach (Guid id in ids) entities.Add(await _CurrencyCosmosRepository.Get(id));
        return entities;
    }
    public async Task Create(Currency Currency)
    {
            StateLogCustomTags stateLogCustomTags = new StateLogCustomTags();
            stateLogCustomTags.Id = Guid.NewGuid();
            stateLogCustomTags.RowId = Currency.Id;
            stateLogCustomTags.TagName = Currency.TagName;
            stateLogCustomTags.TagValue = Currency.TagValue;
            stateLogCustomTags.BranchId = Currency.BranchId;
            stateLogCustomTags.CompanyId = Currency.CompanyId;
            stateLogCustomTags.ProductId = Currency.ProductId;
            stateLogCustomTags.EntityName = "Currency";

        using IDbContextTransaction transaction = _Currencyrepository.Context.Database.BeginTransaction();
        try
        {
            await _Currencyrepository.Add(Currency);
            await _stateLogIndexingRepository.Add(stateLogCustomTags);
            await _CurrencyCosmosRepository.Add(Currency);
            
            transaction.Commit(); 
        } 
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();  
            throw;
        }
    }
    public async Task Create(IEnumerable<Currency> entities)
    {
        foreach (Currency entity in entities) await Create(entity);
    }

    public async Task Update(Currency entity) => await _CurrencyCosmosRepository.Update(entity);

    public async Task Update(List<Currency> entities) => await _CurrencyCosmosRepository.Update(entities);

    public async Task Delete(Currency entity) => await _CurrencyCosmosRepository.Delete(entity);

    public async Task Delete(Guid id)
    {
        Currency entity = await _CurrencyCosmosRepository.Get(id);
        await _CurrencyCosmosRepository.Delete(entity);
    }

    public async Task Delete(IEnumerable<Currency> entities) => await _CurrencyCosmosRepository.Delete(entities);

}