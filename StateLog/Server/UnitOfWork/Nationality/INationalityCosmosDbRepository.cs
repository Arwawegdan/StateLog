namespace StateLog.Server;
public interface INationalityCosmosDbRepository 
{
    Task<Nationality> Add(Nationality entity);
    Task<IEnumerable<Nationality>> Add(IEnumerable<Nationality> entities);

    Task<IEnumerable<Nationality>> Get(string queryString);
    Task<Nationality> Get(Guid id);

    Task<Nationality> Update(Nationality entity);
    Task<IEnumerable<Nationality>> Update(IEnumerable<Nationality> entities);

    Task<Nationality> Delete(Nationality entity);

    Task<IEnumerable<Nationality>> Delete(IEnumerable<Nationality> entities);

    Task<IEnumerable<Nationality>> GetByTagValue(string text);
    Task<IEnumerable<Nationality>> GetByTagName(string text);
}