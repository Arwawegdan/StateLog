namespace StateLog.Server;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StateLogCustomTagsConfiguration())
                    .ApplyConfiguration(new NationalityConfiguration())
                    .ApplyConfiguration(new NationalityReducerConfiguration())
                    .ApplyConfiguration(new EmployeeConfiguration())
                    .ApplyConfiguration(new EmployeeReducerConfiguration());
    }
}