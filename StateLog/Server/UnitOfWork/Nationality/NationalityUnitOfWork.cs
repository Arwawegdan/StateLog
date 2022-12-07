namespace StateLog.Server;
public class NationalityUnitOfWork : INationalityUnitOfWork
{
    private readonly INationalityRepository _nationalityRepository;
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly INationalityCosmosDbRepository _nationalityCosmosRepository;

    public NationalityUnitOfWork(INationalityRepository nationalityRepository, IEmployeeUnitOfWork employeeUnitOfWork ,INationalityCosmosDbRepository  nationalityCosmosRepository)
    { 
        _nationalityRepository = nationalityRepository;
        _stateLogIndexingRepository = new StateLogCustomTagsRepository(nationalityRepository.Context);   
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

        using IDbContextTransaction transaction = _nationalityRepository.Context.Database.BeginTransaction();
        try
        {
            await _nationalityRepository.Add(nationality);
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
    public async Task Update(Nationality nationality)
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

        using IDbContextTransaction transaction = _nationalityRepository.Context.Database.BeginTransaction();
        try
        {
            await _nationalityRepository.Update(nationality);
            await _stateLogIndexingRepository.Update(stateLogCustomTags);
            await _nationalityCosmosRepository.Update(nationality);

            transaction.Commit();
        }
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();
            throw;
        }
    }
    public Employee MapFromNationalityReduerToNationality(EmployeeReducer employeeReducer)
    {
        Employee employee = new Employee();
        employee.PartitionKey = employeeReducer.PartitionKey;
        employee.Name = employeeReducer.Name;
        employee.TagName = employeeReducer.TagName;
        employee.TagValue = employeeReducer.TagValue;
        employee.Id = employeeReducer.Id;
        return employee;
    }
    public async Task Create(IEnumerable<Nationality> entities)
    {
        foreach (Nationality entity in entities) await Create(entity);
    }

    public async Task Update(List<Nationality> entities)
    {
        await _nationalityCosmosRepository.Update(entities);
    }
    public async Task Delete(Nationality entity) => await _nationalityCosmosRepository.Delete(entity);
    public async Task Delete(Guid id)
    {
        using IDbContextTransaction transaction = _nationalityRepository.Context.Database.BeginTransaction();
        try
        {
            await _nationalityRepository.Remove(id); 
            Nationality nationality = await _nationalityCosmosRepository.Get(id);
            await _nationalityCosmosRepository.Delete(nationality);
            transaction.Commit();
    } 
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();  
            throw;
        }

    }
    public async Task Delete(IEnumerable<Nationality> nationalities)
    {
        foreach (Nationality nationality in nationalities)
            await Delete(nationality);
    }
    public async Task UpdateNationalities()
    {
        await _nationalityRepository.UpdateNationalities();     
    }
    public async Task Update(Nationality nationality, int operation = 0)
    {
        await _nationalityRepository.Update(nationality, operation); 
    }
}