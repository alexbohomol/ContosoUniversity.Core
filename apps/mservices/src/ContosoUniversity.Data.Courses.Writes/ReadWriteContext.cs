namespace ContosoUniversity.Data.Courses.Writes;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext(DbContextOptions<ReadWriteContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}
