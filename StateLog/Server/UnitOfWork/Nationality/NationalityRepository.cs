namespace StateLog.Server;
public class NationalityRepository : BaseSettingsRepository<Nationality> , INationalityRepository
{
    public ApplicationDbContext Context { get; }
    
    public NationalityRepository(ApplicationDbContext context) : base(context)
    {
        Context = context;
    }
}