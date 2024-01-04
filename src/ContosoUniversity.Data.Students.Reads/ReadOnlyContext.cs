namespace ContosoUniversity.Data.Students.Reads;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

using Student = Application.Contracts.Repositories.ReadOnly.Projections.Student;

internal class ReadOnlyContext : DbContext
{
    public const string Schema = "std";

    public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<Enrollment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Student>(new EntityTypeConfigurations());
    }
}
