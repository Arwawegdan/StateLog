namespace StateLog.Server;
public class NationalityConfiguration :  BaseSettingsEntityConfiguration<Nationality> , IEntityTypeConfiguration<Nationality>
{
    public virtual new void Configure(EntityTypeBuilder<Nationality> builder)
    {
        builder.ToTable("Nationalities");  
        }
    };
