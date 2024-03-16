namespace ContosoUniversity.Data.Students.Reads;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

using Student = Application.Contracts.Repositories.ReadOnly.Projections.Student;

internal class ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : DbContext(options)
{
    public const string Schema = "std";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<Enrollment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Student>(new EntityTypeConfigurations());
    }
}
