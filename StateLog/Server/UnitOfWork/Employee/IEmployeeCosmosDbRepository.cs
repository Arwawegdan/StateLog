namespace StateLog.Server;
public interface IEmployeeCosmosDbRepository
{
    Task<Employee> Add(Employee entity);
    Task<IEnumerable<Employee>> Add(IEnumerable<Employee> entities);

    Task<IEnumerable<Employee>> Get(string queryString);
    Task<Employee> Get(Guid id);

    Task<Employee> Update(Employee entity);
    Task<IEnumerable<Employee>> Update(IEnumerable<Employee> entities);

    Task<Employee> Delete(Employee entity);
    Task<IEnumerable<Employee>> Delete(IEnumerable<Employee> entities);

    Task<IEnumerable<Employee>> GetByTagValue(string text);
    Task<IEnumerable<Employee>> GetByTagName(string text);
}