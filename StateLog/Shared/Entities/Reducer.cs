namespace StateLog.Shared;
public class Reducer  : BaseSettingsEntity
{
    //public Guid? NationalityId { get; set; }
    //public Queue<Nationality>? Queue { get;  set; }
    public string? PartitionKey { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid CreatorId { get; set; }
    public string? TagName { get; set; }
    public string? TagValue { get; set; }
    public DateTime Datetime { get; set; }
    public string SchemaName { get; set; }

}