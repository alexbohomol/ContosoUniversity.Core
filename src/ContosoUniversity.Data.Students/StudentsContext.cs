namespace ContosoUniversity.Data.Students;

using Microsoft.EntityFrameworkCore;

using Models;

public class StudentsContext : DbContext
{
    private const string Schema = "std";

    public StudentsContext(DbContextOptions<StudentsContext> options) : base(options)
    {
    }

    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>().ToTable("Enrollment", Schema);
        modelBuilder.Entity<Student>().ToTable("Student", Schema);
    }
}