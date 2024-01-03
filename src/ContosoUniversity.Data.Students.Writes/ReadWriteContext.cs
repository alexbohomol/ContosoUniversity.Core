namespace ContosoUniversity.Data.Students.Writes;

using Domain.Student;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext : DbContext
{
    public const string Schema = "std";

    public ReadWriteContext(DbContextOptions<ReadWriteContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration<Enrollment>(new EntityTypeConfigurations());
        modelBuilder.ApplyConfiguration<Student>(new EntityTypeConfigurations());
    }
}
