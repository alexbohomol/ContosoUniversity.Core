namespace ContosoUniversity.Data.Departments.Writes;

using global::Departments.Core.Domain;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext(DbContextOptions<ReadWriteContext> options) : DbContext(options)
{
    public const string Schema = "dpt";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<OfficeAssignment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<CourseAssignment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Instructor>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Department>(new EntityTypeConfigurations());
    }
}
