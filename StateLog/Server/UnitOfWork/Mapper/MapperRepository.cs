namespace StateLog.Server;
public class MapperRepository : BaseRepository<Mapper>, IMapperRepository
{
    public MapperRepository(ApplicationDbContext context) : base(context){}
}