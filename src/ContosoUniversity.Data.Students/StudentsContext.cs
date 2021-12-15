namespace ContosoUniversity.Data.Students;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

public class StudentsContext : DbContext
{
    public const string Schema = "std";

    public StudentsContext(DbContextOptions<StudentsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<Enrollment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Student>(new EntityTypeConfigurations());
    }
}