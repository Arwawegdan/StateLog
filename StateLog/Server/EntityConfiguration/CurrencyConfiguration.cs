namespace StateLog.Server;
public class CurrencyConfiguration :  BaseSettingsEntityConfiguration<Currency> , IEntityTypeConfiguration<Currency>
{
    public virtual new void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currencies");
    }
}