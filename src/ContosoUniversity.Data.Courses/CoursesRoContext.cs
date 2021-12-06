namespace ContosoUniversity.Data.Courses;

using Microsoft.EntityFrameworkCore;

public class CoursesRoContext : DbContext
{
    public CoursesRoContext(DbContextOptions<CoursesRoContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeRoConfigurations());
    }
}