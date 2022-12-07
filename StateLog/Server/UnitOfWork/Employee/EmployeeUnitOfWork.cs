namespace StateLog.Server;
public class EmployeeUnitOfWork : IEmployeeUnitOfWork
{
    private readonly IEmployeeRepository _employeeRepository; 
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly IEmployeeCosmosDbRepository _employeeCosmosRepository;

    public EmployeeUnitOfWork(IEmployeeRepository employeeRepository, IEmployeeCosmosDbRepository employeeCosmosRepository)
    {
        _employeeRepository = employeeRepository;
        _stateLogIndexingRepository = new StateLogCustomTagsRepository(employeeRepository.Context);   
        _employeeCosmosRepository = employeeCosmosRepository;
    }
    public async Task<IEnumerable<Employee>> Read() => await _employeeCosmosRepository.Get($"select * from c where c.partitionKey = {"employee"}");

    public async Task<Employee> Read(Guid id) => await _employeeCosmosRepository.Get(id);

    public async Task<IEnumerable<Employee>> ReadByTagValue(string text)
    { 
        List<Employee> entities = new List<Employee>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagValue(text);
        foreach (Guid id in ids) entities.Add(await _employeeCosmosRepository.Get(id));
        return entities;
    }
    public async Task<IEnumerable<Employee>> ReadByTagName(string text)
    {
        List<Employee> entities = new List<Employee>();
        IEnumerable<Guid?> ids = await _stateLogIndexingRepository.SearchByTagName(text);
        foreach (Guid id in ids) entities.Add(await _employeeCosmosRepository.Get(id));
        return entities;
    }
    public async Task Create(Employee employee)
    {
           
            StateLogCustomTags stateLogCustomTags = new StateLogCustomTags();
            stateLogCustomTags.Id = Guid.NewGuid();
            stateLogCustomTags.RowId = employee.Id;
            stateLogCustomTags.EntityName = "Employee";
            stateLogCustomTags.TagValue = employee.TagValue;
            stateLogCustomTags.TagName = employee.TagName;
           
           

        using IDbContextTransaction transaction = _employeeRepository.Context.Database.BeginTransaction();
        try
        {
            await _employeeRepository.Add(employee);
            await _stateLogIndexingRepository.Add(stateLogCustomTags);
            await _employeeCosmosRepository.Add(employee);
            
            transaction.Commit(); 
        } 
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();  
            throw;
        }
    }
    public async Task Create(IEnumerable<Employee> entities)
    {
        foreach (Employee entity in entities) await Create(entity);
    }

    public async Task Update(Employee entity) => await _employeeCosmosRepository.Update(entity);

    public async Task Update(List<Employee> entities) => await _employeeCosmosRepository.Update(entities);

    public async Task Delete(Employee entity) => await _employeeCosmosRepository.Delete(entity);

    public async Task Delete(Guid id)
    {
        Employee entity = await _employeeCosmosRepository.Get(id);
        await _employeeCosmosRepository.Delete(entity);
    }

    public async Task Delete(IEnumerable<Employee> entities) => await _employeeCosmosRepository.Delete(entities);

}