namespace ContosoUniversity.Data.Students.Reads;

using global::Students.Core.Domain;

using Microsoft.EntityFrameworkCore;

using Student = global::Students.Core.Projections.Student;

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
