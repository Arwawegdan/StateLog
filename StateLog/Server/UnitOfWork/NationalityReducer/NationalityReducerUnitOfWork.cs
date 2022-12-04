namespace StateLog.Server;
public class NationalityReducerUnitOfWork : BaseUnitOfWork<NationalityReducer>, INationalityReducerUnitOfWork
{
    public NationalityReducerUnitOfWork(INationalityReducerRepository nationalityReducerRepository) 
                    : base(nationalityReducerRepository) { }
}