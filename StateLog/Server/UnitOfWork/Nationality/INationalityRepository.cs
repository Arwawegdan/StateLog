namespace StateLog.Server;
public interface INationalityRepository : IBaseSettingsRepository<Nationality>
{
    public ApplicationDbContext Context { get; }
}