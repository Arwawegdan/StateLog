namespace StateLog.Server;
public class NationalityConfiguration :  BaseSettingsEntityConfiguration<Nationality> , IEntityTypeConfiguration<Nationality>
{
    public virtual new void Configure(EntityTypeBuilder<Nationality> builder)
    {
        builder.ToTable("Nationalities");

        builder.HasKey(e => new
        {
            e.Id,
            e.ProductId,
            e.BranchId,
            e.CreatorId,
            e.Name,
            e.TagValue,
            e.TagName,
            e.PartitionKey
        }); 
        }
    };
