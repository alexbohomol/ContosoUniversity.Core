namespace ContosoUniversity.Data.Departments.Writes;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext : DbContext
{
    public const string Schema = "dpt";

    public ReadWriteContext(DbContextOptions<ReadWriteContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<OfficeAssignment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<CourseAssignment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Instructor>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Department>(new EntityTypeConfigurations());
    }
}