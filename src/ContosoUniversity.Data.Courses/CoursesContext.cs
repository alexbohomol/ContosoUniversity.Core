namespace ContosoUniversity.Data.Courses;

using Microsoft.EntityFrameworkCore;

public class CoursesContext : DbContext
{
    public CoursesContext(DbContextOptions<CoursesContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}