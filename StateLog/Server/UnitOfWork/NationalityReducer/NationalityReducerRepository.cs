namespace StateLog.Server;
public class NationalityReducerRepository : BaseRepository<NationalityReducer>, INationalityReducerRepository
{
    public NationalityReducerRepository(ApplicationDbContext context) : base(context)
    { }
}