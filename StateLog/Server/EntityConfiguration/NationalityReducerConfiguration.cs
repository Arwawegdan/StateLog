namespace StateLog.Server;
public class NationalityReducerConfiguration : BaseSettingsEntityConfiguration<NationalityReducer> , IEntityTypeConfiguration<NationalityReducer>
{
    public virtual new void Configure(EntityTypeBuilder<NationalityReducer> builder)
    {
        builder.ToTable("NationalityReducer");
        builder.HasKey(e => new
        {
            e.Id,
            e.Datetime
        });
    } 
}