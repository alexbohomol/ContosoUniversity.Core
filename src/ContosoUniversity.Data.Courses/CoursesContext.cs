namespace ContosoUniversity.Data.Courses;

using Domain.Course;

using Microsoft.EntityFrameworkCore;

public class CoursesContext : DbContext
{
    public CoursesContext(DbContextOptions<CoursesContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}