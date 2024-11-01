namespace Departments.Data.Reads;

using Core.Domain;

using Microsoft.EntityFrameworkCore;

using Department = Core.Projections.Department;
using Instructor = Core.Projections.Instructor;

internal class ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : DbContext(options)
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
