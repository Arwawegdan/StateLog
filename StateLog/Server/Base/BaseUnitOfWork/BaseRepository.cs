namespace StateLog.Server;
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected DbSet<TEntity> dbSet;
    private readonly ApplicationDbContext _context;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Get() => await dbSet.ToListAsync();
    public virtual async Task<TEntity> Get(Guid id) => await
                                          dbSet.FirstOrDefaultAsync(e => e.Id == id) ?? Activator.CreateInstance<TEntity>();

    public virtual async Task Add(TEntity entity)
    {
        if(entity.Id == null || entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        ThrowExceptionIfParameterNotSupplied(entity);

        await dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }
    public virtual async Task Add(IEnumerable<TEntity> entities)
    {
        ThrowExceptionIfParameterNotSupplied(entities);

        await dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public virtual async Task Update(List<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            await Update(entity, false);

        await SaveChangesAsync();
    }

    public virtual async Task Update(TEntity entity, bool isAutosSaveChangesEnabled = true)
    {
        ThrowExceptionIfParameterNotSupplied(entity);

        await ThrowExceptionIfIfEntityExistsInDatabase(entity);

        await Task.Run(() => dbSet.Update(entity));

        if (isAutosSaveChangesEnabled)
            await SaveChangesAsync();
    }

    private async Task ThrowExceptionIfIfEntityExistsInDatabase(TEntity entity)
    {
        if (entity.Id == null && entity.Id != Guid.Empty)
            throw new ArgumentNullException($"Id of {typeof(TEntity).Name} is null or has an empty GUID");

        TEntity? entityFromDb = await Get(entity.Id.Value);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(TEntity)} was not found in DB");
    }

    public virtual async Task Remove(Guid id)
    {
        TEntity? entityFromDb = await Get(id);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(TEntity)} was not found in DB");

        await Task.Run(() => dbSet.Remove(entityFromDb));
        await SaveChangesAsync();
    }
    public virtual async Task Remove(TEntity entity)
    {
        if (entity == null || entity.Id == null)
            throw new ArgumentNullException($"{nameof(TEntity)} was not provided.");

        TEntity? entityFromDb = await Get(entity.Id.Value);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(TEntity)} was not found in DB");

        await Task.Run(() => dbSet.Remove(entity));
        await SaveChangesAsync();
    }
    public virtual async Task Remove(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(TEntity)} was not provided.");

        await Task.Run(() => dbSet.RemoveRange(entities));
        await SaveChangesAsync();
    }

    private static void ThrowExceptionIfParameterNotSupplied(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"{nameof(TEntity)} was not provided.");
    }
    private static void ThrowExceptionIfParameterNotSupplied(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(TEntity)} was not provided.");
    }

    public async Task<IDbContextTransaction> GetTransaction() => await _context.Database.BeginTransactionAsync();

    protected async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}

