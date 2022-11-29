namespace StateLog.Server;
public class BaseSettingsEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>, IEntityTypeConfiguration<TEntity>
    where TEntity : BaseSettingsEntity
{
    public virtual new void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.Name).IsUnique();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50);
    }
}