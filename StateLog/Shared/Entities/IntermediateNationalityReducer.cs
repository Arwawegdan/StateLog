namespace StateLog.Shared;
public class IntermediateNationalityReducer  : BaseSettingsEntity
{
    public string? PartitionKey { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid CreatorId { get; set; }
    public string? TagName { get; set; }
    public string? TagValue { get; set; }
    public DateTime Datetime { get; set; } 

}