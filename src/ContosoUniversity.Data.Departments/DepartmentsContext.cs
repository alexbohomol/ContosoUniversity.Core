namespace ContosoUniversity.Data.Departments;

using Microsoft.EntityFrameworkCore;

using Models;

public class DepartmentsContext : DbContext
{
    private const string Schema = "dpt";

    public DepartmentsContext(DbContextOptions<DepartmentsContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<CourseAssignment> CourseAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().ToTable("Department", Schema);
        modelBuilder.Entity<Instructor>().ToTable("Instructor", Schema);
        modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment", Schema);
        modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment", Schema)
            .HasKey(c => new { c.CourseExternalId, c.InstructorId });
    }
}