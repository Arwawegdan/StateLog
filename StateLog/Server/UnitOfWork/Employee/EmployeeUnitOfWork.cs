namespace StateLog.Server;
public class EmployeeUnitOfWork : IEmployeeUnitOfWork
{
    private readonly IEmployeeRepository _employeeRepository; 
    private readonly IStateLogCustomTagsRepository _stateLogIndexingRepository;  
    private readonly IEmployeeCosmosDbRepository _employeeCosmosRepository;
    private readonly IReducerRepository _reducerRepository;
    private readonly INationalityUnitOfWork _nationalityUnitOfWork;

    public EmployeeUnitOfWork(IEmployeeRepository employeeRepository, IReducerRepository reducerRepository , IEmployeeCosmosDbRepository employeeCosmosRepository)
    {
        _employeeRepository = employeeRepository;
        _stateLogIndexingRepository = new StateLogCustomTagsRepository(employeeRepository.Context);   
        _employeeCosmosRepository = employeeCosmosRepository;
        _reducerRepository = reducerRepository;
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
        Guid nationalityId = employee.NationalityId;
        Nationality nationality = await _nationalityUnitOfWork.Read(nationalityId);
        await _nationalityUnitOfWork.Update(nationality, 1); 
     
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
    public async Task UpdateEmployees()
    {

        using IDbContextTransaction transaction = _employeeRepository.Context.Database.BeginTransaction();
        try
        {
            
            IEnumerable<Reducer> reducer = await _reducerRepository.Get();
            IEnumerable<Reducer> employeeReducer = reducer.Where(e => e.SchemaName == "Employees"); 
            IEnumerable<Employee> employeesFromDatabase = await _employeeRepository.Get();
            employeeReducer = employeeReducer.OrderBy(e => e.Datetime).ToList();
            Employee nationality = new Employee();

            IList<Guid?> guids = employeeReducer.Select(e => e.Id).Distinct().ToList();
            foreach (Guid? guid in guids)
            {
                foreach (Reducer employeeReducerItem in employeeReducer)
                {
                    if (guid == employeeReducerItem.Id)
                        nationality = MapFromEmployeeReducerToNationality(employeeReducerItem);
                }
                //guids.Remove(guid); 
                // no of requests = 2*(guids which have changed)  + etnen fo2 + 
                IEnumerable<Employee> allIds = employeesFromDatabase.Where(e => e.Id == guid); 
         
                await _employeeRepository.Remove(allIds); 
                await _employeeRepository.Add(nationality);
                await _employeeRepository.Context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();
            throw;
        }
    }

    private Employee MapFromEmployeeReducerToNationality(Reducer reducerItem)
    {
        Employee employee = new Employee();
        employee.PartitionKey = reducerItem.PartitionKey;
        employee.Name = reducerItem.Name;
        employee.TagName = reducerItem.TagName;
        employee.TagValue = reducerItem.TagValue;
        employee.Id = reducerItem.Id;
        return employee;
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
        Guid nationalityId = employee.NationalityId;
        Nationality nationality = await _nationalityUnitOfWork.Read(nationalityId);
        await _nationalityUnitOfWork.Update(nationality, -1);
        await _employeeCosmosRepository.Delete(employee);
    }

    public async Task Delete(Guid id)
    {
        Employee entity = await _employeeCosmosRepository.Get(id);
        await _employeeCosmosRepository.Delete(entity);
    }

    public async Task Delete(IEnumerable<Employee> entities) => await _employeeCosmosRepository.Delete(entities);

    public async Task Update(Employee employee)
    {
       // Employee employee = await _employeeRepository.Get()
       //  await _employeeRepository.Update(employee);

    }

}