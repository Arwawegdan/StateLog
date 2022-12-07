namespace StateLog.Server;
public class EmployeeReducerConfiguration : BaseSettingsEntityConfiguration<EmployeeReducer> , IEntityTypeConfiguration<EmployeeReducer>
{
    public virtual new void Configure(EntityTypeBuilder<EmployeeReducer> builder)
    {
        builder.ToTable("EmployeeReducer");
        builder.HasKey(e => new
        {
            e.Id,
            e.Datetime
        });
    } 
}