namespace StateLog.Server;
public interface INationalityRepository : IBaseSettingsRepository<Nationality>
{
    public ApplicationDbContext Context { get; }
    public Task Update(Nationality nationality);
    public Task UpdateNationalities(); 
}