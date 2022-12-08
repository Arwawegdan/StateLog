namespace StateLog.Server; 
public interface IEmployeeUnitOfWork
{
    Task Create(Employee entity);
    Task Create(IEnumerable<Employee> entities);

    Task<IEnumerable<Employee>> Read();
    Task<Employee> Read(Guid id);

    Task Update(Employee entity);
    Task Update(List<Employee> entities);

    Task Delete(Employee entity);
    Task Delete(Guid id); 
    Task Delete(IEnumerable<Employee> entities);

    Task<IEnumerable<Employee>> ReadByTagValue(string text);
    Task<IEnumerable<Employee>> ReadByTagName(string text);
}