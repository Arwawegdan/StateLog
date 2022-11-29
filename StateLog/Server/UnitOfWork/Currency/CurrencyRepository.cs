namespace StateLog.Server;
public class CurrencyRepository : BaseSettingsRepository<Currency> , ICurrencyRepository
{
    public ApplicationDbContext Context { get; }
    
    public CurrencyRepository(ApplicationDbContext context) : base(context)
    {
        Context = context;
    }
}