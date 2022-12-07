namespace StateLog.Server;
public class EmployeeReducerUnitOfWork : BaseUnitOfWork<EmployeeReducer>, IEmployeeReducerUnitOfWork
{
    public EmployeeReducerUnitOfWork(IEmployeeReducerRepository employeeReducerRepository)
                    : base(employeeReducerRepository) { }
}