namespace StateLog.Shared;
public abstract class BaseSettingsEntity : BaseEntity
{
    [JsonProperty(PropertyName = "name")]
    public string? Name { get; set; }
}
