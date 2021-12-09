namespace ContosoUniversity.Data.Departments;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

public class DepartmentsContext : DbContext
{
    public const string Schema = "dpt";

    public DepartmentsContext(DbContextOptions<DepartmentsContext> options) : base(options)
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