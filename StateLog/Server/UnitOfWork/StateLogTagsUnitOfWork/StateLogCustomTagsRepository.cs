namespace StateLog.Server;
public class StateLogCustomTagsRepository : BaseRepository<StateLogCustomTags>, IStateLogCustomTagsRepository
{
    public StateLogCustomTagsRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<IEnumerable<Guid?>> SearchByTagValue(string text) => await dbSet.Where(e => e.TagValue == text).Select(e => e.RowId).ToListAsync();
    public async Task<IEnumerable<Guid?>> SearchByTagName(string text) => await dbSet.Where(e => e.TagName == text).Select(e => e.RowId).ToListAsync();
    public async Task<IEnumerable<Guid?>> SearchByEntityName(string text) => await dbSet.Where(e => e.EntityName == text).Select(e => e.RowId).ToListAsync();
}