namespace StateLog.Server; 
public interface INationalityUnitOfWork 
{
    Task Create(Nationality entity);
    Task Create(IEnumerable<Nationality> entities);

    Task<IEnumerable<Nationality>> Read();
    Task<Nationality> Read(Guid id);

    Task Update(Nationality entity);
    Task Update(List<Nationality> entities);
    Task Update(Nationality nationality, int operation = 0);

    Task Delete(Nationality entity);
    Task Delete(Guid id); 
    Task Delete(IEnumerable<Nationality> entities);

    Task<IEnumerable<Nationality>> ReadByTagValue(string text);
    Task<IEnumerable<Nationality>> ReadByTagName(string text);

    Task UpdateNationalities(); 
}