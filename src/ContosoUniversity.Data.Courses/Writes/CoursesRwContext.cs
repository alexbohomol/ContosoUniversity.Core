namespace ContosoUniversity.Data.Courses.Writes;

using Microsoft.EntityFrameworkCore;

public class CoursesRwContext : DbContext
{
    public CoursesRwContext(DbContextOptions<CoursesRwContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeRwConfigurations());
    }
}