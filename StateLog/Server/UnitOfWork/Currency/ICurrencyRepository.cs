namespace StateLog.Server;
public interface ICurrencyRepository : IBaseSettingsRepository<Currency>
{
    public ApplicationDbContext Context { get; }
}