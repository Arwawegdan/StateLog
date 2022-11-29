namespace StateLog.Server;
public class StateLogCustomTagsConfiguration :  BaseEntityConfiguration<StateLogCustomTags> , IEntityTypeConfiguration<StateLogCustomTags>
{
    public virtual new void Configure(EntityTypeBuilder<StateLogCustomTags> builder)
    {
        builder.ToTable("StateLogCustomTags");
        builder.HasKey(e => new { e.RowId, e.TagName });
    }
}