namespace StateLog.Server;
public interface IStateLogCustomTagsUnitOfWork : IBaseUnitOfWork<StateLogCustomTags>
{
    Task<IEnumerable<Guid?>> SearchByEntityName(string text);
    Task<IEnumerable<Guid?>> SearchByTagName(string text);
    Task<IEnumerable<Guid?>> SearchByTagValue(string text);
}