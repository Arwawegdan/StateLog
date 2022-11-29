namespace StateLog.Server; 
public interface ICurrencyUnitOfWork
{
    Task Create(Currency entity);
    Task Create(IEnumerable<Currency> entities);

    Task<IEnumerable<Currency>> Read();
    Task<Currency> Read(Guid id);

    Task Update(Currency entity);
    Task Update(List<Currency> entities);

    Task Delete(Currency entity);
    Task Delete(Guid id); 
    Task Delete(IEnumerable<Currency> entities);

    Task<IEnumerable<Currency>> ReadByTagValue(string text);
    Task<IEnumerable<Currency>> ReadByTagName(string text);
}