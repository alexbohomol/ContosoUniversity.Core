namespace ContosoUniversity.Data.Students.Writes;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext(DbContextOptions<ReadWriteContext> options) : DbContext(options)
{
    public const string Schema = "std";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<Enrollment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Student>(new EntityTypeConfigurations());
    }
}
