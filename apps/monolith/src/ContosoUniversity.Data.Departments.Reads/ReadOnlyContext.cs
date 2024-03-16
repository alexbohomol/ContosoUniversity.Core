namespace ContosoUniversity.Data.Departments.Reads;

using Application.Contracts.Repositories.ReadOnly.Projections;

using Domain.Instructor;

using Microsoft.EntityFrameworkCore;

using Instructor = Application.Contracts.Repositories.ReadOnly.Projections.Instructor;

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
