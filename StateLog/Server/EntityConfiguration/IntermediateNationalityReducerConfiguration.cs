namespace StateLog.Server;
public class IntermediateNationalityReducerConfiguration : BaseSettingsEntityConfiguration<IntermediateNationalityReducer> , IEntityTypeConfiguration<IntermediateNationalityReducer>
{
    public virtual new void Configure(EntityTypeBuilder<IntermediateNationalityReducer> builder)
    {
        builder.ToTable("IntermediateNationalityReducer");
        builder.HasKey(e => new
        {
            e.Id,
            e.Datetime
        });
    } 
}