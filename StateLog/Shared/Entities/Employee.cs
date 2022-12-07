namespace StateLog.Shared;
public class Employee : BaseSettingsEntity
{
    [JsonProperty(PropertyName = "partitionKey")]
    public string PartitionKey { get; set; } = "employee";

    [JsonProperty(PropertyName = "tagName")]
    public string? TagName { get; set; }

    [JsonProperty(PropertyName = "tagValue")]
    public string? TagValue { get; set; }

    [JsonProperty(PropertyName = "nationalityId")]
    public Guid NationalityId { get; set; }
}