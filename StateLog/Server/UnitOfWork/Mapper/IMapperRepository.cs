namespace StateLog.Server;
public interface IMapperRepository : IBaseRepository<Mapper>
{
      Task Remove(IEnumerable<Mapper> nationalities);
}