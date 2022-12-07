namespace StateLog.Server;
public interface INationalityRepository : IBaseSettingsRepository<Nationality>
{
    public ApplicationDbContext Context { get; }
    public Task Update(Nationality nationality);

    public Task Update(Nationality nationality, int operation = 0);
    public Task UpdateNationalities(); 
}