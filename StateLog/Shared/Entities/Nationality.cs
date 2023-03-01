namespace StateLog.Shared;
public class Nationality  : BaseSettingsEntity
{
    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; } = "nationality";
    
    [JsonProperty(PropertyName = "ProductId")]
    public Guid ProductId { get; set; }

    [JsonProperty(PropertyName = "companyId")]
    public Guid CompanyId { get; set; }

    [JsonProperty(PropertyName = "branchId")]
    public Guid BranchId { get; set; }

    [JsonProperty(PropertyName = "creatorId")]
    public Guid CreatorId { get; set; }

    [JsonProperty(PropertyName = "tagName")]
    public string? TagName { get; set; }

    [JsonProperty(PropertyName = "tagValue")]
    public string? TagValue { get; set; }

    [JsonProperty(PropertyName = "statisticalColoumn")]
    public int StatisticalColoumn { get; set; }   
}