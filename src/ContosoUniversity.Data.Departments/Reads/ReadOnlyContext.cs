namespace ContosoUniversity.Data.Departments.Reads;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

internal class ReadOnlyContext : DbContext
{
    public const string Schema = "dpt";

    public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : base(options)
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