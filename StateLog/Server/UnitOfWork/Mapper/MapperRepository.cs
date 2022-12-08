namespace StateLog.Server;
public class MapperRepository : BaseRepository<Mapper>, IMapperRepository
{
    public MapperRepository(ApplicationDbContext context) : base(context){}

    public async Task Remove(IEnumerable<Mapper> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(Mapper)} was not provided.");

        await Task.Run(() => dbSet.RemoveRange(entities));
        await SaveChangesAsync();
    }
}