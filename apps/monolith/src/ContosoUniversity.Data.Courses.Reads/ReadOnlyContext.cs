namespace ContosoUniversity.Data.Courses.Reads;

using Microsoft.EntityFrameworkCore;

public class ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}
