namespace StateLog.Server;
public class EmployeeRepository : BaseSettingsRepository<Employee> , IEmployeeRepository
{
    public ApplicationDbContext Context { get; }

    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
        Context = context;
    }
}