namespace StateLog.Server;
public class MapperUnitOfWork : BaseUnitOfWork<Mapper>, IMapperUnitOfWork
{
    public MapperUnitOfWork(IMapperRepository mapperRepository) : base(mapperRepository) {}
}