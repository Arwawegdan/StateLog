namespace StateLog.Server;
public interface IStateLogCustomTagsRepository : IBaseRepository<StateLogCustomTags>
{
    Task<IEnumerable<Guid?>> SearchByTagValue(string text);
    Task<IEnumerable<Guid?>> SearchByTagName(string text);
    Task<IEnumerable<Guid?>> SearchByEntityName(string text);
}