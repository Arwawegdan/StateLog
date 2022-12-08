namespace StateLog.Server;
public class ReducerConfiguration : BaseEntityConfiguration<Reducer> , IEntityTypeConfiguration<Reducer>
{
    public virtual new void Configure(EntityTypeBuilder<Reducer> builder)
    {
        builder.ToTable("Reducer");
        builder.HasKey(e => new
        {
            e.Id,
            e.Datetime
        });
    } 
}