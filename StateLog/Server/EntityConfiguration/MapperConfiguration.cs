namespace StateLog.Server;
public class MapperConfiguration : BaseEntityConfiguration<Mapper> , IEntityTypeConfiguration<Mapper>
{
    public virtual new void Configure(EntityTypeBuilder<Mapper> builder)
    {
        builder.ToTable("Mapper");
        builder.HasKey(e => new
        {
            e.Id,
            e.DateTime
        });
    } 
}