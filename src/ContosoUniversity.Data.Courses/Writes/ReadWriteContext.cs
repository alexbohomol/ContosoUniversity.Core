namespace ContosoUniversity.Data.Courses.Writes;

using Microsoft.EntityFrameworkCore;

public class ReadWriteContext : DbContext
{
    public ReadWriteContext(DbContextOptions<ReadWriteContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}