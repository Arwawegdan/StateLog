namespace StateLog.Server;
public class EmployeeReducerRepository : BaseRepository<EmployeeReducer>, IEmployeeReducerRepository
{
    public EmployeeReducerRepository(ApplicationDbContext context) : base(context)
    { }
}