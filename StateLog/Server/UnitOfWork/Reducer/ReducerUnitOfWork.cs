namespace StateLog.Server;
public class ReducerUnitOfWork : BaseUnitOfWork<Reducer>, IReducerUnitOfWork
{
    public ReducerUnitOfWork(IReducerRepository reducerRepository): base(reducerRepository){}
}