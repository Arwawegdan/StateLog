namespace StateLog.Server;
using StateLog.Shared;

public class EmployeeUnitOfWork : IEmployeeUnitOfWork
{
    private readonly IEmployeeRepository _employeeRepository; 
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly IEmployeeCosmosDbRepository _employeeCosmosRepository;
    private readonly IMapperRepository _mapperRepository;

    public EmployeeUnitOfWork(IEmployeeRepository employeeRepository, IEmployeeCosmosDbRepository employeeCosmosRepository, 
                              IMapperRepository mapperRepository)
    {
        _employeeRepository = employeeRepository;
        _stateLogIndexingRepository = new StateLogCustomTagsRepository(employeeRepository.Context);
        _employeeCosmosRepository = employeeCosmosRepository;
        _mapperRepository = mapperRepository;
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
        foreach (Guid id in ids) entities.Add(await _employeeRepository.Get(id));
        return entities;
    }
    public async Task Create(Employee employee)
    {
        StateLogCustomTags stateLogCustomTags = new StateLogCustomTags();
        stateLogCustomTags.Id = Guid.NewGuid();
        stateLogCustomTags.RowId = employee.Id;
        stateLogCustomTags.TagName = employee.TagName;
        stateLogCustomTags.TagValue = employee.TagValue;
        stateLogCustomTags.EntityName = nameof(employee);
        stateLogCustomTags.CompanyId = Guid.NewGuid();
        stateLogCustomTags.BranchId = Guid.NewGuid();
        stateLogCustomTags.CreatorId = Guid.NewGuid();
        stateLogCustomTags.ProductId = Guid.NewGuid();
        stateLogCustomTags.LastModeifierId = Guid.NewGuid();

        stateLogCustomTags.DateTime = DateTime.Now;

        using IDbContextTransaction transaction = _employeeRepository.Context.Database.BeginTransaction();
        try
        {
            await _employeeRepository.Add(employee);
            await _stateLogIndexingRepository.Add(stateLogCustomTags);
            await _employeeCosmosRepository.Add(employee);
            await EmployeeMapper(employee); 

            transaction.Commit(); 
        } 
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();  
            throw;
        }
    }
    public async Task Update(Employee employee)
    {
        Guid nationalityId = employee.NationalityId;        
        StateLogCustomTags stateLogCustomTags = new StateLogCustomTags();
        stateLogCustomTags.Id = Guid.NewGuid();
        stateLogCustomTags.RowId = employee.Id;
        stateLogCustomTags.EntityName = "Employee";
        stateLogCustomTags.TagValue = employee.TagValue;
        stateLogCustomTags.TagName = employee.TagName;

        using IDbContextTransaction transaction = _employeeRepository.Context.Database.BeginTransaction();
        try
        {
            await _employeeRepository.Update(employee);
            await _stateLogIndexingRepository.Update(stateLogCustomTags);
            await _employeeCosmosRepository.Update(employee);

            Employee employeeFromDatabase = await _employeeRepository.Get(employee.Id);
            await EmployeeMapper(employee, employeeFromDatabase.NationalityId, employee.NationalityId);

            transaction.Commit();
        }
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();
            throw;
        }       
    }
    public async Task EmployeeMapper(Employee employee)
    {
        Mapper mapper = new Mapper();
        mapper.SchemaName = nameof(Employee);
        mapper.UpdatedColoumn = nameof(Employee.NationalityId); 
        mapper.ChangedColumnType = ChangedColumnType.Override;
        mapper.ChangedColumnNewValue = "1";
        mapper.DateTime = DateTime.Now;
        mapper.Id = employee.Id;
        mapper.ColoumnId = employee.NationalityId; 
        await _mapperRepository.Add(mapper);
        
    }
    public async Task EmployeeMapper(Employee employee, Guid oldId, Guid newId)
    {
        if (oldId != newId)
        { 
            Mapper mapper = new Mapper();
            mapper.SchemaName = nameof(Employee);
            mapper.UpdatedColoumn = nameof(Employee.NationalityId);
            mapper.ChangedColumnType = ChangedColumnType.Increamnt;
            // m4 fahmaha awy
            mapper.ChangedColumnNewValue = "1"; 
            mapper.DateTime = DateTime.Now;
            mapper.Id = employee.Id;

            await _mapperRepository.Add(mapper); 
        }
    }
    public async Task Create(IEnumerable<Employee> entities)
    {
        foreach (Employee entity in entities) await Create(entity);
    }
    public async Task Update(List<Employee> entities)
    {
        await _employeeCosmosRepository.Update(entities);
    }
    public async Task Delete(Employee employee)
    {
        //Guid nationalityId = employee.NationalityId;
        await _employeeCosmosRepository.Delete(employee);
    }
    public async Task Delete(Guid id)
    {
        Employee entity = await _employeeCosmosRepository.Get(id);
        await _employeeCosmosRepository.Delete(entity);
    }
    public async Task Delete(IEnumerable<Employee> entities)
    {
        await _employeeCosmosRepository.Delete(entities);
       // await _employeeRepository.Remove(entities);
    }
}