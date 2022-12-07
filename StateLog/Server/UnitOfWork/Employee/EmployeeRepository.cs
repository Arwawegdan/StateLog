namespace StateLog.Server;
public class EmployeeRepository : BaseSettingsRepository<Employee> , IEmployeeRepository
{
    public ApplicationDbContext Context { get; }
    private readonly EmployeeReducerRepository _employeeReducerRepository; 

    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        Context = context;
    }
    public async Task Update(Employee employee)
    {
        dbSet.Add(employee);
        await Context.SaveChangesAsync();
        EmployeeReducer nationalityQueue = await QueueMapp(employee);
        await _employeeReducerRepository.Add(nationalityQueue);
    }
    public async Task<EmployeeReducer> QueueMapp(Employee employee)
    {
        EmployeeReducer employeeReducer = new EmployeeReducer();
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