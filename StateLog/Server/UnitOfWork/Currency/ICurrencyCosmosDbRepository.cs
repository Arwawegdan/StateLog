namespace StateLog.Server;
public interface ICurrencyCosmosDbRepository
{
    Task<Currency> Add(Currency entity);
    Task<IEnumerable<Currency>> Add(IEnumerable<Currency> entities);

    Task<IEnumerable<Currency>> Get(string queryString);
    Task<Currency> Get(Guid id);

    Task<Currency> Update(Currency entity);
    Task<IEnumerable<Currency>> Update(IEnumerable<Currency> entities);

    Task<Currency> Delete(Currency entity);
    Task<IEnumerable<Currency>> Delete(IEnumerable<Currency> entities);

    Task<IEnumerable<Currency>> GetByTagValue(string text);
    Task<IEnumerable<Currency>> GetByTagName(string text);
}