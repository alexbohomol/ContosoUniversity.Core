namespace Students.Data.Reads;

using Core.Domain;

using Microsoft.EntityFrameworkCore;

using Student = Core.Projections.Student;

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
