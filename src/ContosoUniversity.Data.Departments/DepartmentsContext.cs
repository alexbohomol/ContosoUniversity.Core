namespace ContosoUniversity.Data.Departments;

using Microsoft.EntityFrameworkCore;

public class DepartmentsContext : DbContext
{
    private const string Schema = "dpt";

    public DepartmentsContext(DbContextOptions<DepartmentsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }
}