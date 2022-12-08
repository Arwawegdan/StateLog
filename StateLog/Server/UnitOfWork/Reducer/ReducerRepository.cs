namespace StateLog.Server;
public class ReducerRepository : BaseRepository<Reducer>, IReducerRepository
{
    public ReducerRepository(ApplicationDbContext context) : base(context){}
}