namespace ContosoUniversity.Data.Courses.Reads;

using Microsoft.EntityFrameworkCore;

internal class ReadOnlyContext : DbContext
{
    public ReadOnlyContext(DbContextOptions<ReadOnlyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("crs");

        modelBuilder.ApplyConfiguration(new EntityTypeConfigurations());
    }
}
