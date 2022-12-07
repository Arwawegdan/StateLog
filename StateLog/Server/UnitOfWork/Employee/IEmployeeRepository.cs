namespace StateLog.Server;
public interface IEmployeeRepository : IBaseSettingsRepository<Employee>
{
    public ApplicationDbContext Context { get; }
}