namespace StateLog.Server;
public class EmployeeConfiguration :  BaseSettingsEntityConfiguration<Employee> , IEntityTypeConfiguration<Employee>
{
    public virtual new void Configure(EntityTypeBuilder<Employee> builder)
    {

        builder.ToTable("Employees");
        //builder.HasOne(e => e.Nationality).WithMany().HasForeignKey(e => e.NationalityId).HasConstraintName("fk-employee-nationality");
        builder.HasKey(e => new
        {
            e.Id,
            e.Name,
            e.TagValue,
            e.TagName,
            e.PartitionKey,
            e.NationalityId
        }); 
        }

    };
