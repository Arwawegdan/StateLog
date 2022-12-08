namespace StateLog.Server;
public class EmployeeRepository : BaseSettingsRepository<Employee> , IEmployeeRepository
{
    public ApplicationDbContext Context { get; }
    private readonly ReducerRepository _reducerRepository; 

    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        Context = context;
    }
    public async Task Update(Employee employee)
    {
        dbSet.Add(employee);
        await Context.SaveChangesAsync();
        Reducer nationalityQueue = await QueueMapp(employee);
        await _reducerRepository.Add(nationalityQueue);
    }
    public async Task<Reducer> QueueMapp(Employee employee)
    {
        Reducer employeeReducer = new Reducer();
        employeeReducer.Id = employee.Id;
        employeeReducer.PartitionKey = employee.PartitionKey;
        employeeReducer.TagValue = employee.TagValue;
        employeeReducer.TagName = employee.TagName;
        employeeReducer.Name = employee.Name;
        employeeReducer.CreatorId = employeeReducer.CreatorId;
        employeeReducer.Datetime = DateTime.Now;
        return employeeReducer;
    }
}